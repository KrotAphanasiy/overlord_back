using System;
using Flash.Central.Foundation.Base.Dtos;
using Flash.Central.Foundation.Base.Models;

namespace Flash.Central.Dtos.DetectionEvent
{
    /// <summary>
    /// Class. Represents detection event  Data Transfer Object.
    /// Derived from BaseGuidDto
    /// </summary>
    public class DetectionEventDto : BaseGuidDto
    {
        /// <summary>
        /// The relation to camera by id
        /// </summary>
        public Guid CameraId { get; set; }
        /// <summary>
        /// The relation to camera's region by id
        /// </summary>
        public long CameraRegionId { get; set; }
        /// <summary>
        /// The relation to gas station by id
        /// </summary>
        public long GasStationId { get; set; }
        /// <summary>
        /// Date and time of event
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Probability of plate number similarity
        /// </summary>
        public double Probability { get; set; }
        /// <summary>
        /// Link to the original image
        /// </summary>
        public string OriginalImageLink { get; set; }
        /// <summary>
        /// Link to the creopped image
        /// </summary>
        public string CroppedImageLink { get; set; }
        /// <summary>
        /// Processed
        /// </summary>
        public bool Processed { get; set; }

    }
}
