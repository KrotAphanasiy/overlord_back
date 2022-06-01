using System.Collections.Generic;
using Flash.Central.Foundation.Base.ViewModels;

namespace Flash.Central.ViewModel.Camera
{
    /// <summary>
    ///   Class. Represent camera's view model
    ///   Derived from BaseGuidVm
    /// </summary>
    public class CameraVm : BaseGuidVm
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
        public long GasStationId { get; set; }
        /// <summary>
        /// The collection of related regions
        /// </summary>
        public List<CameraRegionVm> Regions { get; set; }
    }
}
