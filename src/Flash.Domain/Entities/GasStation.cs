using System.Collections.Generic;
using Flash.Central.Foundation.Base.Entities;

namespace Flash.Domain.Entities
{
    /// <summary>
    ///   Class. Represent gas station entity
    ///   Derived from BaseLongEntity
    /// </summary>
    public class GasStation : BaseLongEntity
    {
        /// <summary>
        /// The name of gas station
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The qauntity of pumps
        /// </summary>
        public int GasPumpCount { get; set; }
        /// <summary>
        /// Collection of related cameras
        /// </summary>
        public virtual List<Camera> Cameras { get; set; }
        /// <summary>
        /// Collection of related terminals
        /// </summary>
        public virtual List<Terminal> Terminals { get; set; }
    }
}
