using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Flash.Central.Foundation.Base.Entities;

namespace Flash.Domain.Entities
{
    /// <summary>
    ///   Class. Represent camera's entity
    ///   Derived from BaseGuidEntity
    /// </summary>
    public class Camera : BaseGuidEntity
    {
        /// <summary>
        /// The name of camera
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// URL to access camera mostly through rstp protocol
        /// </summary>
        public string ConnectionUrl { get; set; }
        /// <summary>
        /// Camera's network address
        /// </summary>
        public string NetworkAddress { get; set; }
        /// <summary>
        /// TCP or UDP port
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Camera's login
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Camera's password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Notes
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// Camera's authentication api key
        /// </summary>
        public string ApiKey { get; set; }
        /// <summary>
        /// The id of gas station
        /// </summary>
        [ForeignKey(nameof(GasStation))]
        public long GasStationId { get; set; }
        public virtual GasStation GasStation { get; set; }

        [InverseProperty(nameof(CameraRegion.Camera))]
        public virtual List<CameraRegion> Regions { get; set; }
    }
}
