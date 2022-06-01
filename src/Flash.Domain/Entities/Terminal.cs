using Flash.Central.Foundation.Base.Entities;

namespace Flash.Domain.Entities
{
    /// <summary>
    ///   Class. Represent terminal entity
    ///   Derived from BaseLongEntity
    /// </summary>
    public class Terminal : BaseLongEntity
    {
        /// <summary>
        /// Code of terminal
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// The name of terminal
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The gas station id
        /// </summary>
        public long GasStationId { get; set; }
        public virtual CameraRegion AssignedCameraRegion { get; set; }
        public virtual GasStation GasStation { get; set; }
    }
}
