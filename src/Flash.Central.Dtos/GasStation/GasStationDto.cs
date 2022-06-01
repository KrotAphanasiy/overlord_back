using Flash.Central.Foundation.Base.Dtos;

namespace Flash.Central.Dtos.GasStation
{
    /// <summary>
    /// Class. Represents gas station  Data Transfer Object.
    /// Derived from BaseLongDto
    /// </summary>
    public class GasStationDto : BaseLongDto
    {
        /// <summary>
        /// The name of gas station
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Number of pump's counts 
        /// </summary>
        public int GasPumpCount { get; set; }
    }
}
