using System.Collections.Generic;
using Flash.Central.Foundation.Base.Models;

namespace Flash.Central.Foundation.Pagination
{
    /// <summary>
    /// Class. Represents pagination model
    /// </summary>
    public class PaginationModel
    {
        /// <summary>
        /// The page number
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// The size of pages
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Collection of sort models
        /// </summary>
        public List<SortModel> Sortings { get; set; }
        /// <summary>
        /// Fi;ter string
        /// </summary>
        public string FilterString { get; set; }
    }
}
