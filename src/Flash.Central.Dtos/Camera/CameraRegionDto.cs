using System;
using Flash.Central.Foundation.Base.Dtos;
using Flash.Central.Foundation.Base.Models;

namespace Flash.Central.Dtos.Camera
{
    /// <summary>
    /// Class. Represents camera's region  Data Transfer Object.
    /// Derived from BaseLongDto
    /// </summary>
    public class CameraRegionDto : BaseLongDto
    {
        /// <summary>
        /// The relation to camera by id
        /// </summary>
        public Guid CameraId
        {
            get;
            set;
        }
        /// <summary>
        /// Direction point by X axis
        /// </summary>
        public int TopLeftX
        {
            get;
            set;
        }
        /// <summary>
        /// Direction point by Y axis
        /// </summary>
        public int TopLeftY
        {
            get;
            set;
        }
        /// <summary>
        /// Direction point by X axis
        /// </summary>
        public int BottomRightX
        {
            get;
            set;
        }
        /// <summary>
        /// Direction point by Y axis
        /// </summary>
        public int BottomRightY
        {
            get;
            set;
        }
        /// <summary>
        /// The name of region
        /// </summary>
        public string? RegionName
        {
            get;
            set;
        }
        /// <summary>
        /// The relation to terminal by id
        /// </summary>
        public long? TerminalId
        {
            get;
            set;
        }
    }
}
