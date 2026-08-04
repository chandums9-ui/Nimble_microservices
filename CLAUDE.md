# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Nimble Property Microservices** — an enterprise .NET 8.0 property management and accounting platform. It follows Clean Architecture with a strict 4-layer numbering convention across 10+ bounded domain contexts.

## Build & Run

```bash
# Build entire solution
dotnet build NimblePropertyMicroServices.sln

# Run a specific API
dotnet run --project "01API/CoreAccounting.API/CoreAccounting.API.csproj"
dotnet run --project "01API/Payable.API/Payable.API.csproj"
dotnet run --project "01API/UserMgmt.API/UserMgmt.API.csproj"

# Run standalone services
dotnet run --project "BankFeedAutoSynchService/BankFeedAutoSynchService.csproj"
dotnet run --project "SchedulePayments/SchedulePayments.csproj"
dotnet run --project "ScheduleBillPayments/ScheduleBillPayments.csproj"
```

There are no Docker, Makefile, or test projects in this solution.

## Architecture

### Layer Convention

All 10 domain contexts follow the same 4-layer folder numbering:

| Folder | Layer | Responsibility |
|--------|-------|----------------|
| `01API/` | Presentation | ASP.NET Core controllers, middleware, Swagger, SignalR hubs |
| `02INFRA/` | Infrastructure | EF Core DbContexts, migrations, repos, AWS S3, Redis, RabbitMQ |
| `03APP/` | Application | Business logic services and orchestration |
| `04DOMAIN/` | Domain | Entity models, DTOs, value objects, message contracts |

Each domain (e.g. `Payable`) has: `Payable.API` → `Payable.Infra` → `Payable.App` → `Payable.Domain` + `Payable.Domain.DTO`.

### Domain Contexts

- **CoreAccounting** — journal entries, ledger, real-time updates via SignalR
- **UserMgmt** — users, authentication, roles (referenced by nearly every other domain)
- **Payable** — vendor bills, AP workflows; references BankFeed.App and UserMgmt.App
- **BankFeed** — bank account aggregation (Plaid/Yodlee/Meld) and sync automation
- **Dashboard** — reporting dashboards and analytics
- **DailySales** — daily revenue tracking
- **ReportsAndConfig** — custom report builder and system configuration
- **WarehouseSynch** — data warehouse synchronization (message consumer side)
- **WHSubscription** — subscription and event routing for warehouse sync
- **DataModel.Domain** — cross-service shared entity models

### Shared Common Projects

These are referenced across all domains — always check here before duplicating logic:

- **Common.API** — base controllers, global exception handler, Swagger config, auth middleware
- **Common.Infra** — `RepositoryBase`, `CommonUnitOfWork`, `AuthDBContext`, `ICacheService`, `IRedisCacheService`, `IElastiCacheService`, `IAWSFileService`, `IURLConnection` (HTTP wrapper), `IPublishService` (MassTransit)
- **Common.App** — shared business services, email service
- **Common.Domain** / **Common.Domain.DTO** — `BaseResponse`, `PagedResponse`, base entities, common enums
- **Messages.Domain** — MassTransit message contracts (`SynchMessage`, `SynchMessage_Error`, AI approval events)
- **Common.Sharing** — shared view models and utilities

### Dependency Injection Pattern

Each layer registers its services via a static extension method called in `Program.cs`:

```csharp
// In Program.cs of any API:
builder.Services.AddCommonInfraServices(builder.Configuration);
builder.Services.AddInfraServices(builder.Configuration);        // domain-specific
builder.Services.AddCoreApplicationServices();                   // App layer
builder.Services.AddAuthInfraServices(builder.Configuration);
```

When adding a new service, add the registration to the domain's `Add*Services` extension method — do not register directly in `Program.cs`.

### Inter-Service Communication

- **Synchronous:** RESTful HTTP via `IURLConnection` (wraps RestSharp); Swagger enabled on all APIs
- **Asynchronous:** MassTransit 8.5.1 over RabbitMQ — exchange `NPDev_SynchX`, consumers in `WHSubscription.Infra` (`VendorEventConsumer`, `PCEventConsumer`, `AccountEventConsumer`)
- **Real-time:** SignalR in `CoreAccounting.API/Hubs/`
- **Database-level:** Multiple SQL Server databases — one per client tenant plus shared auth/UM databases; heavy use of stored procedures (`99SolutionItems/Scripts/`)

### Infrastructure Dependencies

| Concern | Technology |
|---------|-----------|
| ORM | Entity Framework Core 8.0 + SQL Server |
| Caching (local) | StackExchange.Redis 2.8.12, in-memory (5 min sliding / 120 min absolute) |
| Caching (cloud) | AWS ElastiCache (`np-redis-bea7br.serverless.use1.cache.amazonaws.com:6379`, TLS) |
| File storage | AWS S3 (`qa-attachements-np`, `nimbleocrbills`), max 20 MB / 5 files |
| Email | AWS SES via SMTP (`email-smtp.us-east-1.amazonaws.com:465`) |
| Messaging | AWS MQ / RabbitMQ (`b-e19e89b8-7958-44d4-b301-345ae6d80f15.mq.us-east-1.on.aws`) |
| Logging | NLog 5.3.3, configured via `nlog.config` per API |
| Validation | FluentValidation 11.3.0 |
| Mapping | AutoMapper 13.0.1 |

### Third-Party Integrations

- **Bank feeds:** Plaid, Yodlee, Meld (account aggregation)
- **Payments:** Repay (`api.sandbox.cpayplus.com`), E-Signs (ACH/card gateway)
- **Labor:** WrkSpot API (property staff/labor data)
- **OCR/Invoicing:** NimbleIO invoice processing service
- **Chat:** NimbleIO chat rooms

### Multi-Tenant Database Pattern

Connection strings follow a per-client pattern (e.g. `titusgroupConnection`, `opalConnection`, `qa4Connection`). The infrastructure layer selects the correct connection at runtime based on the authenticated tenant context. `UMDBConnection`/`AuthDBConnection` are shared across all tenants.

### Console/Scheduled Apps

Standalone services in the root and `01API/ConsoleApps/` run as Windows Services or scheduled jobs:
- **BankFeedAutoSynchService** — polls and syncs bank feeds via RabbitMQ publish
- **AutoRecurring** — recurring transaction automation
- **SchedulePayments** / **ScheduleBillPayments** — scheduled payment execution
- **WrkSpotIntegration** — pulls labor data from WrkSpot API
- **RepayPaidConsole** — marks Repay payments as settled
