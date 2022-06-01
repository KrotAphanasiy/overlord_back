using System;

namespace Flash.Central.Dtos.MqDtos.Messages
{
    /// <summary>
    /// Class. Represents event's messagess
    /// </summary>
    public class DetectionEventMessage
    {
        /// <summary>
        /// Id of gas station
        /// </summary>
        public long CameraRegionId { get; set; }
        /// <summary>
        /// Date and time of message
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Probability of plate number similarity
        /// </summary>
        public double Probability { get; set; }
        /// <summary>
        /// Riginal image in Base64
        /// </summary>
        public string OriginalImageBase64 { get; set; }
        /// <summary>
        /// Cropped image in Base64
        /// </summary>
        public string CroppedImageBase64 { get; set; }
        /// <summary>
        /// The id of detection event
        /// </summary>
        public Guid DetectionEventId { get; set; }
    }
}
