using System;
using Flash.Central.Foundation.Base.ViewModels;

namespace Flash.Central.ViewModel.DetectionEvent
{
    /// <summary>
    ///  Class. Represent detection events view model
    ///  Derived from BaseGuidModel
    /// </summary>
    public class DetectionEventVm : BaseGuidVm
    {
        /// <summary>
        /// The id of gas station
        /// </summary>
        public long GasStationId { get; set; }
        /// <summary>
        /// The id of camera
        /// </summary>
        public Guid CameraId { get; set; }
        /// <summary>
        /// The id of camera's region
        /// </summary>
        public long CameraRegionId { get; set; }
        /// <summary>
        /// The date of event
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// The link to original image
        /// </summary>
        public string OriginalImageLink { get; set; }
        /// <summary>
        /// The link to cropped image
        /// </summary>
        public string CroppedImageLink { get; set; }
        /// <summary>
        /// Processed
        /// </summary>
        public bool Processed { get; set; }
    }
}
