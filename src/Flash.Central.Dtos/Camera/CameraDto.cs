using System.Collections.Generic;
using Flash.Central.Foundation.Base.Dtos;
using Flash.Central.Foundation.Base.Models;

namespace Flash.Central.Dtos.Camera
{
    /// <summary>
    /// Class. Represents camera's  Data Transfer Object.
    /// Derived from BaseGuidDto
    /// </summary>
    public class CameraDto : BaseGuidDto
    {
        /// <summary>
        /// The name of the camera
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Camera's network address
        /// </summary>
        public string NetworkAddress { get; set; }
        /// <summary>
        /// URL to access camera mostly through rstp protocol
        /// </summary>
        public string ConnectionUrl { get; set; }
        /// <summary>
        /// TCP or UDP port
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Login
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Notes
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// Api key for camera's authentication
        /// </summary>
        public string ApiKey { get; set; }
        /// <summary>
        /// The relation to gas station by id
        /// </summary>
        public long GasStationId { get; set; }
        /// <summary>
        /// Collection related gas stations
        /// </summary>
        public List<CameraRegionDto> Regions { get; set; }
    }
}
