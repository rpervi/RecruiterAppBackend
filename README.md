# Recruitment Backend Microservices (.NET)

This backend uses a small microservice architecture with independent services:

- `Recruitment.JobService` (port `7001`) - job catalog APIs
- `Recruitment.ApplicationService` (port `7002`) - job application APIs
- `Recruitment.NotificationService` (port `7003`) - email notification APIs

All services now use **EF Core + SQL Server** with `DbContext` and auto-create tables on startup (`Database.EnsureCreated()`).

## Database Credentials

Each service has its own database connection string in:

- `Recruitment.JobService/appsettings.json`
- `Recruitment.ApplicationService/appsettings.json`
- `Recruitment.NotificationService/appsettings.json`

Current default local credential placeholder:

- Username: `sa`
- Password: `ChangeMe123!`
- Host: `localhost`
- Port: `1433`

Update these values with your real credentials before production use.

## Recommended Secret Setup (Local)

Do not store real DB passwords in source files. Use `.NET user-secrets`:

```bash
cd Recruitment.JobService
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=recruitment_jobs_db;User Id=sa;Password=YOUR_REAL_PASSWORD;TrustServerCertificate=True;"
```

Repeat the same steps in:

- `Recruitment.ApplicationService`
- `Recruitment.NotificationService`

## Start SQL Server

From `backend` folder:

```bash
docker compose up -d
```

This starts Microsoft SQL Server.

Then run this script in SSMS or `sqlcmd`:

- `database/init.sql`

It creates:

- `recruitment_jobs_db`
- `recruitment_applications_db`
- `recruitment_notifications_db`

## Run Services

Open separate terminals:

```bash
cd Recruitment.JobService
dotnet run --urls http://localhost:7001
```

```bash
cd Recruitment.ApplicationService
dotnet run --urls http://localhost:7002
```

```bash
cd Recruitment.NotificationService
dotnet run --urls http://localhost:7003
```

## Key APIs

- Job Service:
  - `GET /api/jobs`
  - `POST /api/jobs`
- Application Service:
  - `GET /api/applications`
  - `POST /api/applications`
- Notification Service:
  - `GET /api/notifications/email`
  - `POST /api/notifications/email`
