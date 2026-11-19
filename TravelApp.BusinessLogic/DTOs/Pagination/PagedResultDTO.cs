using System;
using System.Collections.Generic;
using System.Text;

namespace TravelApp.BusinessLogic.DTOs.Pagination
{
    /// <summary>Represents a paginated result set.</summary>
    public class PagedResultDTO<T>
    {
        /// <summary>Gets or sets the items in the current page.</summary>
        public List<T> Items { get; set; } = [];

        /// <summary>Gets or sets the total number of items across all pages.</summary>
        public int TotalCount { get; set; }

        /// <summary>Gets or sets the current page number.</summary>
        public int PageNumber { get; set; }

        /// <summary>Gets or sets the number of items per page.</summary>
        public int PageSize { get; set; }

    }
}
