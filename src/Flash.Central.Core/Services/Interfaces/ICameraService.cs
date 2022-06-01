using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Flash.Central.ViewModel.Camera;

namespace Flash.Central.Core.Services.Interfaces
{
    /// <summary>
    /// Interface. Specifies the contract for camera's services.
    /// Derived from IServiceBase.
    /// </summary>
    public interface ICameraService : IServiceBase<CameraModel, CameraVm, Guid>
    {
        /// <summary>
        /// Checks if api key mathes
        /// </summary>
        /// <param name="uid">Camera's guid</param>
        /// <param name="apiKey">Api key</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Boolean value</returns>
        Task<bool> IsMatchApiKey(Guid uid, string apiKey, CancellationToken ct);
        /// <summary>
        /// Gets cameras for certain gas station
        /// </summary>
        /// <param name="gasStationId">Gas station's id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection of cameras</returns>
        Task<List<CameraVm>> GetForStation(long gasStationId, CancellationToken ct = default);
    }
}
