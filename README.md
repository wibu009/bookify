# Bookify

## Introduction

Bookify is a robust, modular, and extensible booking management system designed to demonstrate best practices in Clean Architecture using .NET. The project serves as a practical reference for scalable software design, separation of concerns, and maintainability. Bookify is suitable for learning, prototyping, or as a foundation for production-grade booking applications.

---

## How It Works

Bookify is structured around the principles of Clean Architecture, ensuring that core business logic is independent of frameworks and external agencies. The solution is organized into multiple projects, each with a distinct responsibility:

- **Bookify.Domain**: Contains core business entities, value objects, and domain logic.
- **Bookify.Application**: Houses application-specific services, use-cases, and interfaces for interacting with the domain layer.
- **Bookify.Infrastructure**: Implements external concerns such as data persistence, authentication, authorization, email, caching, and background jobs.
- **Bookify.Api**: The entry point for HTTP requests, exposing RESTful endpoints and handling API-specific concerns.
- **Bookify.Shared**: Contains shared utilities and cross-cutting concerns used across other projects.

The architecture enables easy testing, adaptability to new requirements, and clear boundaries between business logic and infrastructure.

### Main Features

- User, Apartment, Booking, and Review management
- Clean separation of concerns following Clean Architecture
- Dependency Injection throughout the application
- Modular infrastructure (Persistence, Email, Caching, etc.)
- Scalar UI as the default API explorer (modern, interactive documentation)
- API versioning via URL (e.g., `/api/v1/`, `/api/v2/`)
- Docker support for easy deployment

---

## Configuration

### Prerequisites

- [.NET SDK 8+](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (for containerized deployment)

### Getting Started

1. **Clone the repository:**
   ```bash
   git clone https://github.com/wibu009/bookify.git
   cd bookify
   ```

2. **Configure Environment Variables:**
   - Edit `src/Bookify.Api/appsettings.json` and `appsettings.Development.json` for API settings, database connection strings, and other secrets.
   - Review `docker-compose.yml` for containerized environment configuration.

3. **Database Setup:**
   - Ensure your database is running and accessible.
   - Update connection strings as needed.

4. **Run the Application:**
   - Using Docker:
     ```bash
     docker-compose up --build
     ```
   - Using .NET CLI:
     ```bash
     dotnet build
     dotnet run --project src/Bookify.Api/Bookify.Api.csproj
     ```

5. **API Documentation:**
   - Once running, navigate to `/scalar` for interactive API documentation.

---

## Customization

- **Adding Features:** Implement new use-cases in the Application layer, define new entities in the Domain layer, and expose new endpoints via the API project.
- **Infrastructure:** Swap out or extend infrastructure implementations (e.g., database, caching) by updating the Infrastructure project.
- **Testing:** Add unit and integration tests under the `test` directory.

---

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.
