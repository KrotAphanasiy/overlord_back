using System;
using Flash.Central.Foundation.Base.ViewModels;

namespace Flash.Central.ViewModel.RecognitionEvent
{
    /// <summary>
    ///  Class. Represents recognition event view model
    ///  Derived from BaseGuidVm
    /// </summary>
    public class RecognitionEventVm : BaseGuidVm
    {
        /// <summary>
        /// The id of visit
        /// </summary>
        public long VisitId { get; set; }
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
        /// Camera's region name
        /// </summary>
        public string CameraRegionName { get; set; }
        /// <summary>
        /// The date of event
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Plate number
        /// </summary>
        public string PlateNumber { get; set; }
        /// <summary>
        /// Link to image
        /// </summary>
        public string ImageLink { get; set; }
        /// <summary>
        /// Link to processed image
        /// </summary>
        public string ProcessedImageLink { get; set; }
        /// <summary>
        /// True if number is correct
        /// </summary>
        public bool IsIncorrectNumber { get; set; }
    }
}
