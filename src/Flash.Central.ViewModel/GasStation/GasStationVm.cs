using Flash.Central.Foundation.Base.ViewModels;

namespace Flash.Central.ViewModel.GasStation
{
    /// <summary>
    ///  Class. Represents gas station view model
    ///  Derived from BaseLongModel
    /// </summary>
    public class GasStationVm : BaseLongVm
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
