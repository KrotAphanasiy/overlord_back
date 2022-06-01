using Flash.Central.Core.Validation.Interface;
using Flash.Central.ViewModel.DetectionEvent;
using FluentValidation;

namespace Flash.Central.Core.Validation
{
    /// <summary>
    /// Class. Implements contract for all members of IDetectionEventValidator.
    /// Derived from AbstractValidator by FluentValidation with generic type of DetectionEventModel.
    /// </summary>
    public class DetectionEventValidator : AbstractValidator<DetectionEventModel>, IDetectionEventValidator
    {
        /// <summary>
        /// Validates probability greater then 0.5 and less then 1
        /// </summary>
        public DetectionEventValidator()
        {
            // Example validation
            RuleFor(x => x.Probability)
                .GreaterThan(0.5)
                .LessThanOrEqualTo(1);
        }
    }
}
