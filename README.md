# ASP.NET Core Pagination, Sorting, and Searching Examples

This repository contains **two sample projects** demonstrating how to implement **pagination**, **sorting**, and **searching** in ASP.NET Core Web API using different architectural approaches.

---

## 🗂 Project Structure

```plaintext
/SearverSidePaginations
│
├── SimpleArchitecture/         # Project 1: Basic setup
│   └── Controllers/
│       └── CarController.cs    # Pagination, sorting, search directly in controller
│   └── Models/
│   └── Program.cs
│   └── Startup.cs (if applicable)



                              # Project 2: Layered architecture with Repository Pattern



Layered_Pagination.sln
│
├── Presentation/ # API Project
│ └── WebApi # Entry point for the API (Controllers live here)
│
├── Business/ # Business logic layer (Service Layer)
│ ├── Contract/
│ │ ├── IServices/ # Service Interfaces
│ │ └── Services/ # Service Implementations
│ ├── Models/
│ │ ├── Request/ # DTOs for request
│ │ ├── Response/ # DTOs for response
│ │ ├── PaginatedList.cs # Generic pagination result wrapper
│ │ └── PaginationRequest.cs # Request model for paging
│ └── PaginationExtension/
│ ├── FiltersPage.cs # Filtering helpers
│ └── IQueryableExtensions.cs # IQueryable-based pagination/sorting extension methods        # Generic filtering, pagination, sorting (some commented code)
│
├── DataAccess/ # Data access layer
│ ├── Db_Context/ # EF Core DbContext
│ └── Repositories/
│ ├── IRepo/ # Repository Interfaces
│ └── Repo/ # Repository Implementations
│ └── UOW/ # (Optional) Unit of Work pattern
│
├── Entities/ # Domain layer (Entities/Models)
│ ├── Base/ # Base classes like BaseEntity
│ ├── Enums/ # Enum types (if any)
│ └── Car.cs # Main entity used in the sample






## 💡 Future Improvements
- Enable commented-out code for full generic pagination and filtering
- Add more layers such as DTOs, Validators, and Middlewares
- Implement Authentication & Authorization
- 🔜 Add MVC Server-side Pagination Project
- 🔜 Add Clean Architecture Project with Pagination









