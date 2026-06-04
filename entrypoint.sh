#!/bin/bash
# entrypoint.sh - Unified entrypoint for both DB and API containers
# Usage: entrypoint.sh [db|api]

set -e
MODE=$1

if [ "$MODE" = "db" ]; then
    #  SQL Server: Start + Auto-init LMSDatabase
    echo "[DB] Starting SQL Server..."
    /opt/mssql/bin/sqlservr &
    PID=$!

    # Wait for SQL Server to accept connections (max ~120s)
    echo "[DB] Waiting for SQL Server to accept connections..."
    READY=0
    for i in $(seq 1 60); do
        if /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -Q "SELECT 1" &> /dev/null; then
            echo "[DB] SQL Server is ready (took ~$((i * 2))s)."
            READY=1
            break
        fi
        echo "[DB] Still waiting... ($i/60)"
        sleep 2
    done

    if [ "$READY" -ne 1 ]; then
        echo "[DB] ERROR: SQL Server did not start within 120 seconds."
        exit 1
    fi

    # Check if database exists
    # SET NOCOUNT ON prevents "(1 rows affected)" noise
    # tr -d sanitizes whitespace/newlines from sqlcmd output
    DB_EXISTS=$(/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -h -1 -Q \
        "SET NOCOUNT ON; IF DB_ID('LMSDatabase') IS NOT NULL SELECT 1 ELSE SELECT 0" | tr -d '[:space:]')

    if [ "$DB_EXISTS" = "1" ]; then
        echo "[DB] Database 'LMSDatabase' already exists. Skipping initialization."
    else
        echo "[DB] Initializing database from LMSDB.sql..."
        if /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -i /tmp/LMSDB.sql; then
            echo "[DB] Database initialization completed successfully!"
        else
            echo "[DB] ERROR: Database initialization script failed!"
            exit 1
        fi
    fi

    # Keep container running
    wait $PID

elif [ "$MODE" = "api" ]; then
    #  API: Wait for DB, then launch .NET app
    # Note: docker-compose healthcheck (depends_on: condition: service_healthy)
    # already guarantees that DB + tables exist before this container starts.
    # This TCP wait is a safety net for edge cases (e.g., network lag).

    echo "[API] Waiting for database at db:1433..."
    for i in $(seq 1 60); do
        if timeout 1 bash -c "cat < /dev/null > /dev/tcp/db/1433" 2>/dev/null; then
            echo "[API] Database is reachable at db:1433."
            break
        fi
        if [ "$i" -eq 60 ]; then
            echo "[API] ERROR: Database not reachable after 60 seconds."
            exit 1
        fi
        sleep 1
    done

    # Small grace period to ensure SQL Server fully accepts new connections
    sleep 2

    echo "[API] Starting LMS API..."
    exec dotnet PRN232.LMS.API.dll

else
    echo "Usage: entrypoint.sh [db|api]"
    echo "  db  - Start SQL Server and initialize LMSDatabase"
    echo "  api - Wait for database, then start the .NET API"
    exit 1
fi
