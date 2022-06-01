using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flash.Central.Foundation.Base.Entities;

namespace Flash.Domain.Entities
{
    /// <summary>
    ///   Class. Represent detection event entity
    ///   Derived from BaseGuidEntity
    /// </summary>
    public class DetectionEvent : BaseGuidEntity
    {
        /// <summary>
        /// The date of event
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Probability of plate number similarity
        /// </summary>
        public double Probability { get; set; }
        /// <summary>
        /// The link to original image
        /// </summary>
        [MaxLength(100)]
        public string OriginalImageLink { get; set; }
        /// <summary>
        /// The link to cropped image
        /// </summary>
        [MaxLength(100)]
        public string CroppedImageLink { get; set; }

        public virtual CameraRegion CameraRegion { get; set; }
        /// <summary>
        /// The id of camera's region
        /// </summary>
        [ForeignKey(nameof(CameraRegion))]
        public long CameraRegionId { get; set; }
        /// <summary>
        /// True if processed
        /// </summary>
        [DefaultValue(false)]
        public bool Processed { get; set; }
    }
}
