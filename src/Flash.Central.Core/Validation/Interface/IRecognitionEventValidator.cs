using Flash.Central.ViewModel.RecognitionEvent;
using FluentValidation;

namespace Flash.Central.Core.Validation.Interface
{
    /// <summary>
    /// Interface. Specifies the contract for recognition events validation classes.
    /// Derived from IValidator by FluentValidation
    /// </summary>
    public interface IRecognitionEventValidator : IValidator<RecognitionEventModel>
    {

    }
}
