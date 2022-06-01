using Flash.Central.Foundation.Base.Models;

namespace Flash.Central.ViewModel.GasStation
{
    /// <summary>
    ///  Class. Represents gas station model
    ///  Derived from BaseLongModel
    /// </summary>
    public class GasStationModel : BaseLongModel
    {
        /// <summary>
        /// Gas station name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Qauntity of pumps
        /// </summary>
        public int GasPumpCount { get; set; }
    }
}
