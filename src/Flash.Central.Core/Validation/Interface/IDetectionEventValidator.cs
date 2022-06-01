using Flash.Central.ViewModel.DetectionEvent;
using FluentValidation;

namespace Flash.Central.Core.Validation.Interface
{
    /// <summary>
    /// Interface. Specifies the contract for detection events validation classes.
    /// Derived from IValidator by FluentValidation
    /// </summary>
    public interface IDetectionEventValidator : IValidator<DetectionEventModel>
    {

    }
}
