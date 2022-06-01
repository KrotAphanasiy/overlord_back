using System.Collections.Generic;
using Flash.Central.Foundation.Base.Models;

namespace Flash.Central.Foundation.Pagination
{
    /// <summary>
    /// Class. Represents pagination arguments.
    /// </summary>
	public class PaginationArgs
	{

		public PaginationArgs()
		{
			_sortings = new List<SortModel>();
		}

        /// <summary>
        /// The number of page
        /// </summary>
		public int PageNumber { get; set; }
        /// <summary>
        /// The size of pages
        /// </summary>
		public int PageSize { get; set; }
        /// <summary>
        /// Page's sorting
        /// </summary>
		public List<SortModel> Sortings {
			get => _sortings;
            set
			{
				if (value == null)
				{
					value = new List<SortModel>();
				}
				_sortings = value;
			}
		}

		private List<SortModel> _sortings;
	}
}
