using Flash.Central.Foundation.Enums;

namespace Flash.Central.Foundation.Base.Models
{
    /// <summary>
    /// Class. Represents sorting 
    /// </summary>
    public class SortModel
    {
        /// <summary>
        /// The name of field
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Sort direction
        /// </summary>
        public SortDirections Direction { get; set; }
    }
}
