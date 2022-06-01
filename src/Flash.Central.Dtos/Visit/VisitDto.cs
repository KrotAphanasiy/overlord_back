using System;
using System.Collections.Generic;
using Flash.Central.Dtos.GasStation;
using Flash.Central.Dtos.RecognitionEvent;
using Flash.Central.Foundation.Base.Dtos;
using Flash.Central.Foundation.Base.Models;

namespace Flash.Central.Dtos.Visit
{
    /// <summary>
    /// Class. Represents visit Data Transfer Object.
    /// Derived from BaseLongDto
    /// </summary>
    public class VisitDto : BaseLongDto
    {
        /// <summary>
        /// The date and time of event start
        /// </summary>
        public DateTime Start { get; set; }
        /// <summary>
        /// The date and time of event end
        /// </summary>
        public DateTime End { get; set; }
        /// <summary>
        /// Quantity of events
        /// </summary>
        public int EventsCount { get; set; }
        /// <summary>
        /// Plate number
        /// </summary>
        public string PlateNumber { get; set; }
        /// <summary>
        /// True if the visit is correct
        /// </summary>
        public bool IsIncorrectVisit { get; set; }
        /// <summary>
        /// The percent of assurance of similarity
        /// </summary>
        public double PercentAssurance { get; set; }
        /// <summary>
        /// The id of gas station
        /// </summary>
        public long GasStationId { get; set; }
        /// <summary>
        /// The name of gas station
        /// </summary>
        public string GasStationName { get; set; }
        /// <summary>
        /// True if is clean
        /// </summary>
        public bool IsClean { get; set; }
        /// <summary>
        /// Trustworthy
        /// </summary>
        public bool Trustworthy { get; set; }
        /// <summary>
        /// The link to original picture
        /// </summary>
        public string FullImageLink { get; set; }
    }
}
