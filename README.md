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
- Database (SQL Server) running via Docker.
- API running inside Docker containers.
- Includes `Dockerfile` and `docker-compose.yml`.

### 7. Documentation
- Full Swagger/OpenAPI integration for endpoint listing and testing.

---
*Evaluation focuses on architectural integrity, model separation, and RESTful compliance.*
