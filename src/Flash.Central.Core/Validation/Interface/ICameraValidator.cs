using Flash.Central.ViewModel.Camera;
using FluentValidation;

namespace Flash.Central.Core.Validation.Interface
{
    /// <summary>
    /// Interface. Specifies the contract for camera validation classes.
    /// Derived from IValidator by FluentValidation
    /// </summary>
    public interface ICameraValidator : IValidator<CameraModel>
    {

    }
}
