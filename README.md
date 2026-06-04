# PRN232 - Lab 1: Learning Management System (LMS) REST API

## Project Overview
Develop an ASP.NET Core RESTful API using a 3-layer architecture for a Learning Management System (LMS).

## Technical Requirements & Design Standards

### 1. Architecture & Project Structure
Apply a **3-layer architecture**:
- **API Layer** (Controllers)
- **Service Layer** (Business Logic)
- **Repository Layer** (Data Access)

**Naming Convention:**
- `PRN232.LMS.API`
- `PRN232.LMS.Services`
- `PRN232.LMS.Repositories`

### 2. Data Model Specification
The project uses 4 model types:
- **Entity Model**: Database mapping.
- **Business Model**: Business processing.
- **Request Model**: Client input.
- **Response Model**: API output.

### 3. RESTful API Design
- Follow RESTful principles.
- Resource-based endpoints with plural nouns in URLs (e.g., `/api/students`).
- Consistent response format:
```json
{
  "success": true,
  "message": "Request processed successfully",
  "data": {},
  "errors": null
}
```

### 4. Advanced Features
All list APIs support:
- **Searching**: Filter data by keyword.
- **Sorting**: Multi-field ascending/descending order.
- **Paging**: Page number and size with pagination metadata.
- **Selection**: Client can request specific fields (`?fields=id,name`).
- **Expansion**: Include related entities (`?expand=student,course`).

### 5. Database Schema
- **Semester**: Id, Name, StartDate, EndDate.
- **Course**: Id, Name, SemesterId.
- **Subject**: Id, Code, Name, Credit.
- **Student**: Id, FullName, Email, DateOfBirth.
- **Enrollment**: Id, StudentId, CourseId, EnrollDate, Status.

### 6. Docker Deployment
- Database (SQL Server 2022) and API run fully inside Docker containers.
- Database schema and seed data are **automatically initialized** from `LMSDB.sql` on first startup.
- No manual database restore or SQL import required.
- Includes `Dockerfile`, `docker-compose.yml`, and unified `entrypoint.sh`.

## Quick Start (Docker)

**Prerequisites:** Docker and Docker Compose installed.

```bash
git clone <repository-url>
cd PRN232_LMS
docker compose up --build
```

The system will:
1. Start SQL Server 2022 in a container.
2. Automatically create the `LMSDatabase` and import all tables + seed data from `LMSDB.sql`.
3. Wait for the database to be fully ready (healthcheck).
4. Start the ASP.NET Core API on port **5288**.

Once started:
- **Swagger UI**: [http://localhost:5288/swagger](http://localhost:5288/swagger)
- **API Base URL**: `http://localhost:5288/api`
- **SQL Server**: `localhost:1434` (sa / see `.env`)

> **Note:** Database is only initialized on the first run. Subsequent restarts preserve existing data.

To stop and clean up:
```bash
docker compose down       # Stop containers (keep data)
docker compose down -v    # Stop containers and delete database volume
```

### 7. Documentation
- Full Swagger/OpenAPI integration for endpoint listing and testing.

---
*Evaluation focuses on architectural integrity, model separation, and RESTful compliance.*
