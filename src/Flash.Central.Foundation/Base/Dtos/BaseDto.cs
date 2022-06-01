using System;

namespace Flash.Central.Foundation.Base.Dtos
{
    /// <summary>
    /// Class. Represents base Data Transfer Object.
    /// </summary>
    public class BaseDto
    {
        /// <summary>
        /// True if deleted
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// The date of creation
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// The date of update
        /// </summary>
        public DateTime UpdatedDate { get; set; }
    }
}
