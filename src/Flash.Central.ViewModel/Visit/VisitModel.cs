using System;
using System.Collections.Generic;
using System.ComponentModel;
using Flash.Central.Foundation.Base.Models;

namespace Flash.Central.ViewModel.Visit
{
    /// <summary>
    ///  Class. Represent visit model
    ///  Derived from BaseLongModel
    /// </summary>
    public class VisitModel : BaseLongModel
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
        /// True if visit is correct
        /// </summary>
        public bool IsIncorrectVisit { get; set; }
        /// <summary>
        /// The id of gas station
        /// </summary>
        public long GasStationId { get; set; }
        /// <summary>
        /// The name of gas station
        /// </summary>
        public string GasStationName { get; set; }
        /// <summary>
        /// The percent of assurance
        /// </summary>
        public double PercentAssurance { get; set; }

        [DefaultValue(false)]
        public bool IsClean { get; set; }

        [DefaultValue(true)]
        public bool Trustworthy { get; set; }


    }
}
