#  License Manager Minimal API

A lightweight **ASP.NET Core 8 Minimal API** for managing software licenses. The project demonstrates how to build RESTful APIs using the Minimal API approach with **Entity Framework Core**, **SQL Server**, **Serilog**, and **Swagger/OpenAPI**.

---

##  Features

*  CRUD operations for software licenses
*  Search licenses by license key
*  Retrieve active licenses
*  SQL Server integration with Entity Framework Core
*  Automatic API documentation using Swagger
*  Structured logging with Serilog
*  Log storage in:

  * Console
  * File
  * SQL Server
*  Global exception handling middleware
*  Problem Details responses for consistent API error handling
*  Built with ASP.NET Core 8 Minimal APIs

---

##  Built With

* ASP.NET Core 8
* C#
* Minimal APIs
* Entity Framework Core
* SQL Server
* Serilog
* Swagger / OpenAPI

---

##  Project Structure

```text
LicenseManagerMinimalAPI
│
├── Data/
│   └── AppDatabase.cs
│
├── Middleware/
│   └── ExceptionMiddleware.cs
│
├── Models/
│   └── AppLicence.cs
│
├── Migrations/
│
├── logs/
│
├── Program.cs
├── appsettings.json
└── LicenseManagerMinimalAPI.csproj
```

---

##  License Model

Each license contains the following information:

| Property   | Description        |
| ---------- | ------------------ |
| Id         | License identifier |
| UserNmae   | Owner username     |
| Key        | License key        |
| IsActive   | License status     |
| CreateDate | Creation date      |

---

##  Getting Started

### Prerequisites

* .NET 8 SDK
* SQL Server
* Visual Studio 2022 (recommended)

---

### Installation

Clone the repository:

```bash
git clone https://github.com/MoayadMadhoun/License-Manager.git
```

Navigate to the project:

```bash
cd License-Manager
```

Restore dependencies:

```bash
dotnet restore
```

Update the connection string inside:

```text
appsettings.json
```

Apply database migrations:

```bash
dotnet ef database update
```

Run the project:

```bash
dotnet run
```

---

##  API Documentation

After running the application, Swagger UI is available at:

```
https://localhost:<port>/swagger
```

---

##  Available Endpoints

| Method | Endpoint               | Description                |
| ------ | ---------------------- | -------------------------- |
| GET    | `/license`             | Get all licenses           |
| GET    | `/license/active`      | Get active licenses        |
| GET    | `/license/{id}`        | Get license by ID          |
| GET    | `/license/search?key=` | Search by license key      |
| POST   | `/license`             | Create a new license       |
| PUT    | `/license/{id}`        | Update an existing license |
| DELETE | `/license/{id}`        | Delete a license           |

---

##  Logging

The project uses **Serilog** for structured logging.

Logs are written to:

* Console
* `logs/log.txt`
* SQL Server (`Logs` table)

---

##  Error Handling

The project includes:

* Custom Exception Middleware
* ASP.NET Problem Details
* Centralized error responses
* Request logging

---

##  Technologies Demonstrated

* Minimal APIs
* Dependency Injection
* Entity Framework Core
* SQL Server
* Swagger/OpenAPI
* Middleware
* Structured Logging
* RESTful API Design


```text
docs/
├── swagger.png
├── endpoints.png
└── database.png
```

---

##  Contributing

Contributions are welcome.

1. Fork the repository.
2. Create a feature branch.
3. Commit your changes.
4. Push the branch.
5. Open a Pull Request.

---

##  License

This project is intended for educational and learning purposes.

---

##  Author

Developed by **Moayad Madhoun**

https://github.com/MoayadMadhoun
