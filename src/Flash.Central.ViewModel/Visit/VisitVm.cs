using System;
using Flash.Central.Foundation.Base.ViewModels;

namespace Flash.Central.ViewModel.Visit
{
    /// <summary>
    ///  Class. Represent visit view model
    ///  Derived from BaseLongVm
    /// </summary>
    public class VisitVm : BaseLongVm
    {
        /// <summary>
        /// The date of visit start
        /// </summary>
        public DateTime Start { get; set; }
        /// <summary>
        /// The date of visit end
        /// </summary>
        public DateTime End { get; set; }
        /// <summary>
        /// The quantity of events
        /// </summary>
        public int EventsCount { get; set; }
        /// <summary>
        /// Plate number
        /// </summary>
        public string PlateNumber { get; set; }
        /// <summary>
        /// The id of gas station
        /// </summary>
        public long GasStationId { get; set; }
        /// <summary>
        /// The name of gas station
        /// </summary>
        public string GasStationName { get; set; }
        /// <summary>
        /// True if visit is correct
        /// </summary>
        public bool IsIncorrectVisit { get; set; }
        public bool IsClean { get; set; }
        public bool Trustworthy { get; set; }
        /// <summary>
        /// The link to original picture
        /// </summary>
        public string FullImageLink { get; set; }
    }
}
