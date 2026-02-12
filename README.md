# PropertyCare
PropertyCare is a portfolio backend project designed to demonstrate production-grade ASP.NET Core architecture and engineering practices.
It models a **property maintenance management system** where tenants, owners, and contractors interact through maintenance workflows.

## Features
* Clean Architecture multi-project solution
* ASP.NET Core Web API (.NET 10)
* EF Core + PostgreSQL
* JWT Authentication
* Role-based authorization
* Soft deletes
* Automatic timestamps
* Audit logging
* Pagination + filtering
* Domain-driven status transitions
* Dockerized database setup

### Domain Concepts
* Users (Tenant / Contractor / Admin)
* Properties
* Maintenance Requests
* Audit Logs

## Architecture
```
src/
  PropertyCareApi.Web            -> ASP.NET host, controllers
  PropertyCareApi.Application    -> DTOs, interfaces
  PropertyCareApi.Domain         -> Entities & domain logic
  PropertyCareApi.Infrastructure -> EF Core, persistence
```

Dependency flow:
```
Domain
  ^
Application
  ^
Infrastructure
  ^
Web
```

## Tech Stack
* .NET 10
* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* Docker / Docker Compose
* BCrypt password hashing
* Fluent API entity configuration

## Prerequisites
Install the following:
* [.NET SDK 10](https://dotnet.microsoft.com/download)
* Docker Desktop
* EF CLI tools
```bash
dotnet tool install --global dotnet-ef
```

## Running Locally
### 1. Clone the repo
```bash
git clone <repo-url>
cd PropertyCare
```

### 2. Start PostgreSQL
```bash
docker compose up -d
```

This starts the database container defined in `docker-compose.yml`.

### 3. Apply database migrations
From the repo root:
```bash
dotnet ef database update \
  --project src/PropertyCare.Infrastructure \
  --startup-project src/PropertyCare.Web
```

### 4. Run the API
```bash
dotnet run --project src/PropertyCare.Web
```

openAPI will be available at:
```
https://localhost:xxxx/openapi/v1.json
```

## Configuration
Configuration lives in:
```
src/PropertyCare.Web/appsettings.json
```

Important values:
* Connection string
* JWT secret / issuer / audience

For production-like setups, override via environment variables.

## Database Reset (Optional)
If you need a clean state:
```bash
docker compose down -v
docker compose up -d

dotnet ef database update \
  --project src/PropertyCare.Infrastructure \
  --startup-project src/PropertyCare.Web
```

## Project Goals
This project is used to deepen expertise in:

* ASP.NET Core backend architecture
* EF Core performance patterns
* Authorization models
* Observability and logging
* Testing strategies
* Event-driven evolution

It intentionally evolves toward production-level patterns without premature enterprise complexity.

## Planned Enhancements

* Resource-based authorization
* Global validation + error handling
* Integration testing with containers
* Structured logging (Serilog)
* Background processing
* Redis caching
* Messaging (RabbitMQ)
* Identity provider integration