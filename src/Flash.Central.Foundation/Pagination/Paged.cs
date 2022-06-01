using System.Collections.Generic;

namespace Flash.Central.Foundation.Pagination
{
    /// <summary>
    /// Class. Represents properties for pagination.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class Paged<TItem>
    {
        /// <summary>
        /// Constructor. Initializes parameters.
        /// </summary>
        /// <param name="data">Collection of paged datas</param>
        /// <param name="total">Total quabtity</param>
        /// <param name="pageNumber">Number of page</param>
        /// <param name="pageSize">Page's size</param>
        public Paged(List<TItem> data, int total, int pageNumber, int pageSize)
        {
            Data = data;
            Total = total;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        /// <summary>
        /// Collection of paged datas
        /// </summary>
        public List<TItem> Data { get; set; }
        /// <summary>
        /// Total quabtity
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// Number of page
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Page's size
        /// </summary>
        public int PageSize { get; set; }
    }
}
