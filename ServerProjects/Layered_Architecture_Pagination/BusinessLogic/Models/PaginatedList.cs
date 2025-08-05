using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
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

}
