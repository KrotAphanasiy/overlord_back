using Flash.Central.Foundation.Base.Models;

namespace Flash.Central.ViewModel.GasStation
{
    /// <summary>
    ///  Class. Represents terminal model
    ///  Derived from BaseLongModel
    /// </summary>
    public class TerminalModel : BaseLongModel
    {
        /// <summary>
        /// Termina;'s code
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
        /// The id of gas station
        /// </summary>
        public long GasStationId { get; set; }
    }
}
