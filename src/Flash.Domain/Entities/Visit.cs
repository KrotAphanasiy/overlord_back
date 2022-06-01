using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Flash.Central.Foundation.Base.Entities;

namespace Flash.Domain.Entities
{
    /// <summary>
    ///   Class. Represent terminal entity
    ///   Derived from BaseLongEntity
    /// </summary>
    public class Visit : BaseLongEntity
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
        /// The qauntity of events
        /// </summary>
        public int EventsCount { get; set; }
        /// <summary>
        /// Plate number
        /// </summary>
        public string PlateNumber { get; set; }
        /// <summary>
        /// The id of gas station
        /// </summary>
        [ForeignKey (nameof (GasStation))]
        public long GasStationId { get; set; }
        /// <summary>
        /// Percent of assurance
        /// </summary>
        public double PercentAssurance { get; set; }
        /// <summary>
        /// True if visit is correct
        /// </summary>
        public bool IsIncorrectVisit { get; set; }

        [DefaultValue(false)]
        public bool IsClean { get; set; }

        [DefaultValue(true)]
        public bool Trustworthy { get; set; }

        public virtual GasStation GasStation { get; set; }
        /// <summary>
        /// The collection of related recognition events
        /// </summary>
        public virtual List<RecognitionEvent> RecognitionEvents { get; set; }
    }
}
