# 🎓 AcademicGateway-Backend

> Central RESTful Web API, domain state orchestrator, and relational backend powering the [AcademicGateway](https://github.com/TarekMineRoyal/AcademicGateway) ecosystem.

`AcademicGateway-Backend` is built on **.NET 8** and **C#**, implementing **Clean Architecture** and **Domain-Driven Design (DDD)** principles centered around **Command Query Responsibility Segregation (CQRS)**. It manages user authentication, curriculum governance, project templates, Work Breakdown Structure (WBS) tracking, supervision workflows, provider verification, and real-time synchronization with the [AcademicGateway-AI](https://github.com/TarekMineRoyal/AcademicGateway-AI) engine.

---

## 🏗 Architecture Highlights

### 🌐 System Integration
This service functions as the central core of the platform:
* **AcademicGateway-Backend (This Repo):** Manages relational persistence (PostgreSQL), core business rules, domain events, authentication, and HTTP workflow orchestration.
* **AcademicGateway-AI:** Consumes backend data payloads via HTTP sync for vector embeddings, storage in LanceDB, and semantic matchmaking queries.

Key architectural patterns in this service include:

* **Clean Architecture Layers**: Enforces unidirectional dependencies flowing inward across `Api` $\rightarrow$ `Infrastructure` $\rightarrow$ `Application` $\rightarrow$ `Domain`.
* **CQRS with MediatR Pipeline**: Decouples write commands from read queries using MediatR. Cross-cutting concerns (validation via FluentValidation, transactional boundaries) are injected directly into pipeline behaviors.
* **Domain Event Handling**: Domain events publish entity state changes internally, triggering domain handlers to keep aggregates synchronized within DbContext transaction boundaries.
* **Relational Persistence**: Uses EF Core with Npgsql PostgreSQL provider and automatic `snake_case` naming conventions for database tables and columns.
* **Identity & Security**: ASP.NET Core Identity with custom claim-based JWT authentication (`role`, `sub`), supporting role-based authorization policies across endpoints.

---

## 📂 Project Structure

```text
AcademicGateway-Backend/
├── AcademicGateway.sln           # Master Visual Studio solution file
├── Api/                          # Web API presentation layer
│   ├── Features/                 # Vertical slice REST controllers (Identity, Projects, Students, etc.)
│   ├── Common/                   # Shared API response contracts
│   ├── Infrastructure/           # Exception handling and custom API middleware
│   ├── Program.cs                # Entry point, dependency injection, & DB seeding triggers
│   └── appsettings.json          # Production & development runtime configuration
├── Application/                  # Application business logic layer
│   ├── Features/                 # MediatR command and query handlers grouped by domain
│   ├── Common/                   # Request behaviors, pipeline interfaces, & FluentValidation rules
│   └── DependencyInjection.cs    # Assembly registration scanner
├── Domain/                       # Enterprise core domain layer
│   ├── Subdomains/               # Pure domain entities, aggregates, & domain events
│   └── Common/                   # Entity primitives, value objects, & core exceptions
├── Infrastructure/               # External tech-stack implementations
│   ├── Persistence/              # ApplicationDbContext, EF Core mappings, & database seeders
│   ├── Identity/                 # ASP.NET Core Identity models & current user providers
│   ├── Services/                 # AcademicGateway-AI HTTP client & external adapters
│   └── Migrations/               # PostgreSQL EF Core migration history
├── Application.UnitTests/        # Unit test suite for CQRS handlers and validators
├── Domain.UnitTests/             # Unit test suite for domain aggregates and domain rules
└── IntegrationTests/             # WebApplicationFactory end-to-end integration tests
```

---

## ⚙️ Prerequisites & Setup

### Prerequisites
* **.NET 8.0 SDK** or later
* **PostgreSQL** database instance (local or Docker container)
* **AcademicGateway-AI** microservice (optional, required only for semantic search features)

### Installation

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/TarekMineRoyal/AcademicGateway-Backend.git](https://github.com/TarekMineRoyal/AcademicGateway-Backend.git)
   cd AcademicGateway-Backend
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore AcademicGateway.sln
   ```

3. **Configure the Database Connection:**
   Update `ConnectionStrings:DefaultConnection` inside `Api/appsettings.json` (or `appsettings.Development.json`):
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=AcademicGatewayDb;Username=postgres;Password=YourPassword"
   }
   ```

4. **Apply Database Migrations:**
   ```bash
   dotnet ef database update --project Infrastructure --startup-project Api
   ```
   > **Note:** Seed data (admin accounts, default platform roles, and curriculum constants) automatically seeds on startup when running in non-testing environments.

---

## 🎛 Configuration

Key runtime environment variables managed via `appsettings.json` or environment overrides:

| Configuration Key | Default / Example | Description |
| :--- | :--- | :--- |
| `ConnectionStrings:DefaultConnection` | `Host=localhost;Port=5432;Database=...` | PostgreSQL connection string. |
| `JwtSettings:Secret` | `[Your256BitSecretKeyHere]` | Symmetric key used for JWT signing and verification. |
| `JwtSettings:Issuer` | `AcademicGatewayApi` | Token issuer claim value. |
| `JwtSettings:Audience` | `AcademicGatewayUsers` | Token audience claim value. |
| `JwtSettings:ExpiryMinutes` | `60` | Token expiration lifetime in minutes. |
| `AiEngine:BaseUrl` | `http://localhost:8000` | Endpoint URL for the `AcademicGateway-AI` microservice. |
| `AiEngine:TimeoutInSeconds` | `10` | HTTP timeout threshold for AI microservice requests. |

---

## 🚀 Running the Application

### Local .NET CLI
To start the REST API server locally:

```bash
dotnet run --project Api
```

The application runs on local profiles:
* **HTTP:** `http://localhost:5082`
* **HTTPS:** `https://localhost:7053`
* **Swagger UI:** `http://localhost:5082/swagger`

---

## 🐳 Containerization

For containerized deployments, the backend can be packaged using Docker.

### Dockerfile
The build uses multi-stage targeting in `Api/Dockerfile`.

### Build & Run Commands

```bash
# Build Docker image
docker build -t academicgateway-backend -f Api/Dockerfile .

# Run container with environment overrides
docker run -d -p 5082:80 \
  -e ConnectionStrings__DefaultConnection="Host=host.docker.internal;Port=5432;Database=AcademicGatewayDb;Username=postgres;Password=YourPassword" \
  -e AiEngine__BaseUrl="[http://host.docker.internal:8000](http://host.docker.internal:8000)" \
  --name gateway-backend academicgateway-backend
```

---

## 🧪 Testing Instructions

The repository includes unit test suites for domain logic and CQRS commands, along with integration tests powered by `WebApplicationFactory`.

Execute the entire test suite:

```bash
dotnet test
```

Execute a specific test project:

```bash
# Domain Unit Tests
dotnet test Domain.UnitTests/Domain.UnitTests.csproj

# Application Handler Unit Tests
dotnet test Application.UnitTests/Application.UnitTests.csproj

# Integration Tests
dotnet test IntegrationTests/IntegrationTests.csproj
```

---

## 📄 API Documentation

When running in development mode, dynamic Swagger and OpenAPI specification endpoints are generated at:
* **Interactive Swagger UI:** `http://localhost:5082/swagger`

---

## 🔗 Related Repositories

| Repository | Description |
| :--- | :--- |
| **[AcademicGateway](https://github.com/TarekMineRoyal/AcademicGateway)** | Master documentation hub and system architecture blueprints. |
| **[AcademicGateway-Backend](https://github.com/TarekMineRoyal/AcademicGateway-Backend)** | Primary web application, business logic, and relational backend API *(this repository)*. |
| **[AcademicGateway-Frontend](https://github.com/TarekMineRoyal/AcademicGateway-Frontend)** | User Web Application & UI client. |
| **[AcademicGateway-AI](https://github.com/TarekMineRoyal/AcademicGateway-AI)** | Vector search and semantic matchmaking microservice. |

---

## 🌐 Project Ecosystem

This backend service is one component of the broader **AcademicGateway** platform:

| Repository | Role | Direct Connection to this Service? |
| :--- | :--- | :--- |
| **[AcademicGateway-Backend](https://github.com/TarekMineRoyal/AcademicGateway-Backend)** | Core API & Business State | *(This repository)* |
| **[AcademicGateway-Frontend](https://github.com/TarekMineRoyal/AcademicGateway-Frontend)** | User Web Application | **Yes** — Consumes REST endpoints and authenticates via JWT. |
| **[AcademicGateway-AI](https://github.com/TarekMineRoyal/AcademicGateway-AI)** | Vector Search Microservice | **Yes** — Communicates over HTTP via `AiEngine` settings for vector sync and semantic recommendations. |
