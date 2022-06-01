using Flash.Central.Core.Validation.Interface;
using Flash.Central.ViewModel.GasStation;
using FluentValidation;

namespace Flash.Central.Core.Validation
{
    /// <summary>
    /// Class. Implements contract for all members of ITerminalValidator.
    /// Derived from AbstractValidator by FluentValidation with generic type of TerminalModel.
    /// </summary>
    public class TerminalValidator : AbstractValidator<TerminalModel>, ITerminalValidator
    {

    }
}
