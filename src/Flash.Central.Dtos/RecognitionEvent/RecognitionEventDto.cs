using System;
using Flash.Central.Foundation.Base.Dtos;
using Flash.Central.Foundation.Base.Models;

namespace Flash.Central.Dtos.RecognitionEvent
{
    /// <summary>
    /// Class. Represents recognition event Data Transfer Object.
    /// Derived from BaseGuidDto
    /// </summary>
    public class RecognitionEventDto : BaseGuidDto
    {
        /// <summary>
        /// The id of visit
        /// </summary>
        public long VisitId { get; set; }
        /// <summary>
        /// The id of camera
        /// </summary>
        public Guid CameraId { get; set; }
        /// <summary>
        /// The id of camera's region
        /// </summary>
        public long CameraRegionId { get; set; }
        /// <summary>
        /// The name of camera's region
        /// </summary>
        public string CameraRegionName { get; set; }
        /// <summary>
        /// The id of gas station
        /// </summary>
        public long GasStationId { get; set; }
        /// <summary>
        /// The date and time of recognition event
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Probability of plate number similarity
        /// </summary>
        public double Probability { get; set; }
        /// <summary>
        /// Plate number
        /// </summary>
        public string PlateNumber { get; set; }
        /// <summary>
        /// The link to image
        /// </summary>
        public string ImageLink { get; set; }
        /// <summary>
        /// The link to processed image
        /// </summary>
        public string ProcessedImageLink { get; set; }
        /// <summary>
        /// True if the plate number is correct
        /// </summary>
        public bool IsIncorrectNumber { get; set; }
    }
}
