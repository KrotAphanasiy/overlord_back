using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flash.Central.Foundation.Base.Entities;

namespace Flash.Domain.Entities
{
    /// <summary>
    ///   Class. Represent recognition events entity
    ///   Derived from BaseGuidEntity
    /// </summary>
    public class RecognitionEvent : BaseGuidEntity
    {
        /// <summary>
        /// Plate number
        /// </summary>
        [MaxLength (10)]
        public string PlateNumber { get; set; }
        /// <summary>
        /// Probability of plate number similarity
        /// </summary>
        public double Probability { get; set; }
        /// <summary>
        /// The date and time of event
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Link to image
        /// </summary>
        [MaxLength (100)]
        public string ImageLink { get; set; }
        /// <summary>
        /// Link to processed image
        /// </summary>
        [MaxLength (100)]
        public string ProcessedImageLink { get; set; }
        /// <summary>
        /// The id of camera's region
        /// </summary>
        [ForeignKey (nameof (CameraRegion))]
        public long CameraRegionId { get; set; }
        /// <summary>
        /// True if number is correct
        /// </summary>
        public bool IsIncorrectNumber { get; set; }
        public virtual CameraRegion CameraRegion { get; set; }
        /// <summary>
        /// The id of visit
        /// </summary>
        [ForeignKey (nameof (Visit))]
        public long? VisitId { get; set; }
        public virtual Visit Visit { get; set; }
    }
}
