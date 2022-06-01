using System;
using Flash.Central.Foundation.Base.Models;

namespace Flash.Central.ViewModel.Camera
{
    /// <summary>
    ///   Class. Represent camera's region model
    /// </summary>
    public class CameraRegionModel : BaseLongModel
    {
        /// <summary>
        /// Camera's id
        /// </summary>
        public Guid CameraId
        {
            get;
            set;
        }
        /// <summary>
        /// The id of terminal
        /// </summary>
        public long? TerminalId
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


    }
}
