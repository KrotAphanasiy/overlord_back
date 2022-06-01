using Flash.Central.Core.Validation.Interface;
using Flash.Central.ViewModel.Camera;
using FluentValidation;

namespace Flash.Central.Core.Validation
{
    /// <summary>
    /// Class. Implements contract for all members of ICameraRegionValidator.
    /// Derived from AbstractValidator by FluentValidation with generic type of CameraRegionModel.
    /// </summary>
    public class CameraRegionValidator : AbstractValidator<CameraRegionModel>, ICameraRegionValidator
    {
        public CameraRegionValidator()
        {
        }
    }
}
