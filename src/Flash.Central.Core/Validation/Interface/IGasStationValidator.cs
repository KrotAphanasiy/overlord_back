using Flash.Central.ViewModel.GasStation;
using FluentValidation;

namespace Flash.Central.Core.Validation.Interface
{
    /// <summary>
    /// Interface. Specifies the contract for gas station validation classes.
    /// Derived from IValidator by FluentValidation
    /// </summary>
    public interface IGasStationValidator : IValidator<GasStationModel>
    {

    }
}
