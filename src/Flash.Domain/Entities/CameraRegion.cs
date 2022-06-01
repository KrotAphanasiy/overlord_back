using System;
using System.ComponentModel.DataAnnotations.Schema;
using Flash.Central.Foundation.Base.Entities;

namespace Flash.Domain.Entities
{
    /// <summary>
    ///   Class. Represent camera's region entity
    ///   Derived from BaseLongEntity
    /// </summary>
    public class CameraRegion : BaseLongEntity
    {
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
        /// The id of terminal
        /// </summary>
        [ForeignKey(nameof(WatchingTerminal))]
        public long? TerminalId
        {
            get;
            set;
        }
        /// <summary>
        /// The id of camera
        /// </summary>
        [ForeignKey(nameof(Camera))]
        public Guid CameraId
        {
            get;
            set;
        }
        public Camera Camera
        {
            get;
            set;
        }
        public Terminal WatchingTerminal
        {
            get;
            set;
        }
    }
}
