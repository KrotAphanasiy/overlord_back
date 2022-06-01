using System;
using System.Text.Json.Serialization;

namespace Flash.Central.Foundation.Base.Models
{
    /// <summary>
    /// Class. Represents base model class.
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// True if deleted
        /// </summary>
        [JsonIgnore]
        public bool IsDeleted { get; set; }
        /// <summary>
        /// The date of creation date
        /// </summary>
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// The date of update
        /// </summary>
        [JsonIgnore]
        public DateTime UpdatedDate { get; set; }
    }
}
