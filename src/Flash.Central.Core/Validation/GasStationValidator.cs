using Flash.Central.Core.Validation.Interface;
using Flash.Central.ViewModel.GasStation;
using FluentValidation;

namespace Flash.Central.Core.Validation
{
    /// <summary>
    /// Class. Implements contract for all members of IGasStationValidator.
    /// Derived from AbstractValidator by FluentValidation with generic type of GasStationModel.
    /// </summary>
    public class GasStationValidator : AbstractValidator<GasStationModel>, IGasStationValidator
    {

    }
}
