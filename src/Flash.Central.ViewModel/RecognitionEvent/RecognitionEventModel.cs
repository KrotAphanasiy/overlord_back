using System;
using Flash.Central.Foundation.Base.Models;

namespace Flash.Central.ViewModel.RecognitionEvent
{
    /// <summary>
    ///  Class. Represents recognition event model
    ///  Derived from BaseGuidModel
    /// </summary>
    public class RecognitionEventModel : BaseGuidModel
    {
        /// <summary>
        /// The id of camera's region
        /// </summary>
        public long CameraRegionId { get; set; }
        /// <summary>
        /// The date of event
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Plate number
        /// </summary>
        public string PlateNumber { get; set; }
        /// <summary>
        /// Probability of plate number similarity
        /// </summary>
        public double Probability { get; set; }
        /// <summary>
        /// String in Base64 of original picture
        /// </summary>
        public string ImageBase64 { get; set; }
        /// <summary>
        /// String in Base64 of processed picture
        /// </summary>
        public string ProcessedImageBase64 { get; set; }
        /// <summary>
        /// The id of detection event
        /// </summary>
        public Guid DetectionEventId { get; set; }
    }
}
