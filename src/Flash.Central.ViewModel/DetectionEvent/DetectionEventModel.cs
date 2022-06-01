using System;
using Flash.Central.Foundation.Base.Models;

namespace Flash.Central.ViewModel.DetectionEvent
{
    /// <summary>
    ///  Class. Represent detection events model
    ///  Derived from BaseGuidModel
    /// </summary>
    public class DetectionEventModel : BaseGuidModel
    {
        /// <summary>
        /// The id of camera region
        /// </summary>
        public long CameraRegionId { get; set; }
        /// <summary>
        /// The date of event
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Probability of plate number similarity
        /// </summary>
        public double Probability { get; set; }
        /// <summary>
        /// String in Base64 of original picture
        /// </summary>
        public string OriginalImageBase64 { get; set; }
        /// <summary>
        /// String in Base64 of cropped picture
        /// </summary>
        public string CroppedImageBase64 { get; set; }
        /// <summary>
        /// Processed
        /// </summary>
        public bool Processed { get; set; }
    }
}
