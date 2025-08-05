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
├── Presentation/               # API Project
│   └── WebApi                  # Entry point for the API (Controllers live here)
│
├── Business/                   # Business logic layer (Service Layer)
│   ├── Contract/
│   │   ├── IServices/          # Service Interfaces
│   │   └── Services/           # Service Implementations
│   ├── Models/
│   │   ├── Request/            # DTOs for request
│   │   ├── Response/           # DTOs for response
│   │   ├── PaginatedList.cs    # Generic pagination result wrapper
│   │   └── PaginationRequest.cs# Request model for paging
│   └── PaginationExtension/
│       ├── FiltersPage.cs      # Filtering helpers
│       └── IQueryableExtensions.cs # IQueryable-based pagination/sorting extension methods
│                                 # Generic filtering, pagination, sorting (some commented code)
├── DataAccess/                 # Data access layer
│   ├── Db_Context/             # EF Core DbContext
│   └── Repositories/
│       ├── IRepo/              # Repository Interfaces
│       └── Repo/               # Repository Implementations
│   └── UOW/                    # (Optional) Unit of Work pattern
│
├── Entities/                   # Domain layer (Entities/Models)
│   ├── Base/                   # Base classes like BaseEntity
│   ├── Enums/                  # Enum types (if any)
│   └── Car.cs                  # Main entity used in the sample
```

---

## Server Projects

- ✅ **Simple Architecture:** Logic directly in controllers (search, sort, paginate)
- ✅ **Layered Architecture:** Clean separation using Services, Repositories, Models, and Extensions

---
 Pagination Request 
---
```csharp

 public class PagedRequest<T>
 {
     public int PageIndex { get; set; }  // Default to the first page
     public int PageSize { get; set; } // Default to 10 items per page
     public string? SortColumn { get; set; } // Column name to sort by
     public bool IsAscending { get; set; } = true; // Default sorting order is ascending

     public string? SearchQuery { get; set; } = ""; // Default empty search query
     public T? RequestData { get; set; } = default; // Default value for RequestData
 }
```

 Pagination Response
---
```csharp
    public class PaginatedList<T>
    {
        // Properties
        public int TotalCount { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }

        // Computed property for total pages
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        // Computed properties for pagination navigation
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public List<T> Items { get; private set; }

        // Constructor
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            Items = items ?? new List<T>(); // Ensure Items is never null
            TotalCount = count;
            PageIndex = pageIndex < 1 ? 1 : pageIndex; // Ensure PageIndex is at least 1
            PageSize = pageSize > 0 ? pageSize : 10;   // Default PageSize to 10 if a non-positive value is provided
        }
    }

```

Generic Methods Codes
---
```csharp

    public static class IQueryableExtensions
    {
        // Method to paginate the IQueryable
        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(
            this IQueryable<T> query,
            int pageIndex,
            int pageSize)
        {

            // Ensure pageIndex and pageSize are valid
            if (pageIndex < 1)
                throw new ArgumentOutOfRangeException(nameof(pageIndex), "Page index must be greater than 0.");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than 0.");

            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new PaginatedList<T>(items, totalCount, pageIndex, pageSize);
        }

        // Method for applying sorting
        public static IQueryable<T> Sort<T>(this IQueryable<T> query, string? sortColumn, bool isAscending)
        {
            if (string.IsNullOrWhiteSpace(sortColumn))
                return query; // No sorting if sort column is not provided

            // Create a parameter expression for the entity type
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, sortColumn);

            // Create a lambda expression: x => x.SortColumn
            var lambda = Expression.Lambda(property, parameter);

            string methodName = isAscending ? "OrderBy" : "OrderByDescending";
            var method = typeof(Queryable).GetMethods()
                .Where(m => m.Name == methodName && m.GetParameters().Length == 2)
                .Single()
                .MakeGenericMethod(typeof(T), property.Type);

            return (IQueryable<T>)method.Invoke(null, new object[] { query, lambda });
        }

        public static IQueryable<T> Search<T>(this IQueryable<T> query, string searchQuery, IEnumerable<string> properties)
        {
            if (string.IsNullOrWhiteSpace(searchQuery) || !properties.Any())
                return query;

            // Create a parameter expression for the entity type (represents 'x')
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression combinedExpression = null;

            // Loop through each property and build the search expression
            foreach (var property in properties)
            {
                // Property access: x.PropertyName
                var propertyAccess = Expression.Property(parameter, property);

                // Add null check for the property
                var nullCheck = Expression.NotEqual(propertyAccess, Expression.Constant(null));

                // String.StartsWith method for strict left-side matching
                var startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                var searchValue = Expression.Constant(searchQuery.ToLower()); // Ensure case-insensitive comparison
                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);

                // Call .ToLower() on the property (x.PropertyName.ToLower())
                var toLowerExpression = Expression.Call(propertyAccess, toLowerMethod);

                // Call .StartsWith on the property (x.PropertyName.ToLower().StartsWith(searchQuery))
                var startsWithExpression = Expression.Call(toLowerExpression, startsWithMethod, searchValue);

                // Combine null check and startsWith expression: x.PropertyName != null && x.PropertyName.ToLower().StartsWith(searchQuery)
                var fullCondition = Expression.AndAlso(nullCheck, startsWithExpression);

                // Combine with OR for each property
                combinedExpression = combinedExpression == null
                    ? fullCondition
                    : Expression.OrElse(combinedExpression, fullCondition);
            }

            // Return the filtered query
            return query.Where(Expression.Lambda<Func<T, bool>>(combinedExpression, parameter));
        }

        // Helper method for sorting
    }

```

## Client Projects

- 🚧 Coming Soon: **Angular client** for consuming paginated API

---

## 💡 Future Improvements

- Enable commented-out code for full generic pagination and filtering
- Add more layers such as DTOs, Validators, and Middlewares
- Implement Authentication & Authorization
- 🔜 Add MVC Server-side Pagination Project
- 🔜 Add Clean Architecture Project with Pagination
