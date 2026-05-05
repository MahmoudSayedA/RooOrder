<p align="center">
  <img src="docs/assets/RooOrder_logo_noBg.png" alt="Logo" width="400"/>
</p>
 
 # Roo Order – Backend API

Roo Order is a food ordering system that connects customers with restaurants, allowing users to browse menus, place orders, and track order status in real time.
The system is designed with scalability in mind and can support mobile and web clients.

## Remote Access Dev

Api is hosting now on a remote server, and can be accessed via the following URL:
- [link](http://roo-order.runasp.net)
```
http://roo-order.runasp.net
```
## API Documentation
- [link](http://roo-order.runasp.net/swagger/index.html)
```
http://roo-order.runasp.net/swagger/index.html
```
---

## System Overview

The backend exposes a RESTful API consumed by a mobile application.

Restaurants are responsible for preparing and delivering orders.

The system does not directly manage delivery personnel at this stage.

## 🛠️ Development Commands
### build the database
```
# Create migration
dotnet ef migrations add InitialCreate --project src/Infrastructure

# Update database
dotnet ef database update --project src/Infrastructure

# Remove migration
dotnet ef migrations remove --project src/Infrastructure
```

### Run
```bash
# Run normally
dotnet run --project src/Web

# Run with hot reload
dotnet watch run --project src/Web

# Run in development mode
ASPNETCORE_ENVIRONMENT=Development dotnet run
```

### Test
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/UnitTests

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

Navigate to `https://localhost:7215/swagger`


## 🛠️ Tech Stack

| Layer | Technologies |
|-------|-------------|
| **Framework** | .NET 10 |
| **API** | ASP.NET Core Web API, Swagger/OpenAPI |
| **Database** | SQL Server, Entity Framework Core 10 |
| **Caching** | Redis (StackExchange.Redis) with Polly Circuit Breaker |
| **Authentication** | ASP.NET Core Identity, JWT Bearer |
| **CQRS** | MediatR 12.5 |
| **Validation** | FluentValidation 12.1 |
| **Mapping** | AutoMapper 14.0 |
| **Logging** | Serilog (Console + File sinks) |
| **Background Jobs** | Hangfire 1.8 |
| **Resilience** | Polly 8.6 |
| **Identifiers** | ULID 1.4 |

## 🏗️ Architecture Highlights

### Architecture & Patterns
- ✅ **Clean Architecture** - Separation of concerns with clear layer boundaries
- ✅ **CQRS Pattern** - Command Query Responsibility Segregation with MediatR
- ✅ **Repository Pattern** - Clean data access abstraction
- ✅ **Domain-Driven Design** - Rich domain models with ULID identifiers

### Infrastructure
- ✅ **Redis Caching** - High-performance distributed caching with Polly circuit breaker for resilience
- ✅ **SQL Server** - Entity Framework Core with code-first migrations
- ✅ **Hangfire** - Background job processing and scheduling
- ✅ **JWT Authentication** - Secure token-based authentication with ASP.NET Core Identity

### Developer Experience
- ✅ **AutoMapper** - Object-to-object mapping
- ✅ **FluentValidation** - Elegant validation rules
- ✅ **Serilog** - Structured logging to console and file
- ✅ **Swagger/OpenAPI** - Interactive API documentation
- ✅ **Dynamic LINQ** - Runtime query building
- ✅ **Docker Support** - Containerization ready

### Reliability
- ✅ **Circuit Breaker Pattern** - Polly-based resilience for external services
- ✅ **Graceful Degradation** - App continues working when Redis is unavailable
- ✅ **Error Handling** - Comprehensive exception handling middleware


## 🐳 Docker Support

```bash
# Build and run with Docker Compose
docker-compose up --build

# Run specific services
docker-compose up db redis api

# View logs
docker-compose logs -f api
```

## 📝 API Documentation

Once running, access interactive API documentation at:
- **Swagger UI**: `https://localhost:7215/swagger`
- **Hangfire Dashboard**: `https://localhost:7215/hangfire`

## 🔐 Authentication Flow

1. **Register**: `POST /api/auth/register`
2. **Confirm Email**: `GET /api/auth/confirm-email`
3. **Login**: `POST /api/auth/login` → Returns JWT token
4. **Use Token**: Add `Authorization: Bearer {token}` header to requests

## 🚦 Resilience Features

### Redis Circuit Breaker
- Opens after 2 consecutive failures
- Stays open for 30 seconds
- Automatically retries when half-open
- App continues working without cache when circuit is open

### Connection Timeouts
- Redis: 1 second connect timeout
- Operations: 500ms timeout with Polly


## Future Enhancements
- Support  DeploymentTo Azure App Service
- Support Kubernetes
- Implement GraphQL API alongside REST
- 

## 📚 Resources

- [Mahmoud Sayed Clean Arch](https://github.com/MahmoudSayedA/StartUp)

## Support
- You can reach out for support via GitHub Issues on the repository page.
- Or contact me at: [email](mailto:mahmoudsayed1332002@gmail.com)



## 📄 License

MIT License - See [LICENSE.txt](LICENSE.txt) for details

---

**Built with ❤️ using Best Practice principles**
