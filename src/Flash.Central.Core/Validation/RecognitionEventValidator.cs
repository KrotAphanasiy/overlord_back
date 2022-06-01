using Flash.Central.Core.Validation.Interface;
using Flash.Central.ViewModel.RecognitionEvent;
using FluentValidation;

namespace Flash.Central.Core.Validation
{
    /// <summary>
    /// Class. Implements contract for all members of IRecognitionEventValidator.
    /// Derived from AbstractValidator by FluentValidation with generic type of RecognitionEventModel.
    /// </summary>
    public class RecognitionEventValidator : AbstractValidator<RecognitionEventModel>, IRecognitionEventValidator
    {
        /// <summary>
        /// Validates probability greater then 0.5 and less then 1
        /// </summary>
        public RecognitionEventValidator()
        {
            // Example validation
            RuleFor(x => x.Probability)
                .GreaterThan(0.5)
                .LessThanOrEqualTo(1);
        }
    }
}
