using Business.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.PagiantionExtension
{
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

}
