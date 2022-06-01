using Flash.Central.ViewModel.Camera;
using FluentValidation;

namespace Flash.Central.Core.Validation.Interface
{
    /// <summary>
    /// Interface. Specifies the contract for camera's region validation classes.
    /// Derived from IValidator by FluentValidation
    /// </summary>
    public interface ICameraRegionValidator : IValidator<CameraRegionModel>
    {

    }
}
