# GestionCapteurs

A .NET Core Web API project for managing Capteurs with support for API versioning, caching, and robust error handling.

## Features

* **CRUD Operations**: Create, Read, Update, Delete sensors.
* **API Versioning**: Support for multiple API versions.
* **Caching**: In-memory caching for improved performance.
* **Error Handling**: Custom error codes and detailed error messages.
* **Swagger Integration**: API documentation and testing interface.
* **Entity Framework Core**: Database interactions with PostgreSQL.

## Technologies

* .NET 6
* Entity Framework Core
* PostgreSQL
* Swagger (Swashbuckle)
* xUnit (for testing)
* API Versioning
* In-Memory Caching

## Getting Started

### Prerequisites

1. Install the .NET SDK.
2. Install PostgreSQL and set up a database (e.g., `GestionCapeur`).

### Setup



1. **Install Dependencies**
```bash
dotnet restore
```

2. **Set up Database Connection**
Check `appsettings.json` for connection string:

```

3. **Apply Migrations**
```bash
dotnet ef migrations add (NameMigration)
dotnet ef database update
```

4. **Run the Application**
```bash
dotnet run
```

5. **Access the API**
   * Swagger UI: http://localhost:5000/swagger
   * Default port: `http://localhost:5000`

## Endpoints

### Version 1.0

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/Capteur` | Get all sensors. |
| GET | `/api/v1/Capteur/{id}` | Get a sensor by ID. |
| POST | `/api/v1/Capteur` | Create a new sensor. |
| PUT | `/api/v1/Capteur/{id}` | Update a sensor by ID. |
| DELETE | `/api/v1/Capteur/{id}` | Delete a sensor by ID. |

## Error Handling

* **Custom Error Codes**: Responses include specific error codes (e.g., `CapteurNotFound`, `InvalidData`).
* **Global Error Middleware**: Handles unhandled exceptions and returns structured error responses.

## Testing

This project uses `xUnit` for automated testing.

1. Run tests:
```bash
dotnet test
```

2. Example test coverage:
   * Validates CRUD operations for the `CapteurController`.
   * Verifies caching logic and API versioning.

