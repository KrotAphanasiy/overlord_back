using Flash.Central.Foundation.Base.Dtos;

namespace Flash.Central.Dtos.GasStation
{
    /// <summary>
    /// Class. Represents terminal  Data Transfer Object.
    /// Derived from BaseLongDto
    /// </summary>
    public class TerminalDto : BaseLongDto
    {
        /// <summary>
        /// Terminal's code
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
        /// Relation to gas station by id
        /// </summary>
        public long GasStationId { get; set; }
    }
}
