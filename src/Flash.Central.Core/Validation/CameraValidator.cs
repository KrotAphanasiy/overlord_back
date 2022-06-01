using Flash.Central.Core.Validation.Interface;
using Flash.Central.ViewModel.Camera;
using FluentValidation;

namespace Flash.Central.Core.Validation
{
    /// <summary>
    /// Class. Implements contract for all members of ICameraValidator.
    /// Derived from AbstractValidator by FluentValidation with generic type of CameraModel.
    /// </summary>
    public class CameraValidator : AbstractValidator<CameraModel>, ICameraValidator
    {
        public CameraValidator()
        {

        }
    }
}
