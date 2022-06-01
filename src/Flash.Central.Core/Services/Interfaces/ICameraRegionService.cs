using Flash.Central.ViewModel.Camera;
using Flash.Domain.Entities;

namespace Flash.Central.Core.Services.Interfaces
{
    /// <summary>
    /// Interface. Specifies the contract for camera's region services.
    /// Derived from IServiceBase.
    /// </summary>
    public interface ICameraRegionService : IServiceBase<CameraRegionModel, CameraRegionVm, long>
    {
    }
}
