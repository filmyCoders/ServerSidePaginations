using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{


    public class PagedRequest<T>
    {
        public int PageIndex { get; set; }  // Default to the first page
        public int PageSize { get; set; } // Default to 10 items per page
        public string? SortColumn { get; set; } // Column name to sort by
        public bool IsAscending { get; set; } = true; // Default sorting order is ascending

        public string? SearchQuery { get; set; } = ""; // Default empty search query
        public T? RequestData { get; set; } = default; // Default value for RequestData
    }
}
