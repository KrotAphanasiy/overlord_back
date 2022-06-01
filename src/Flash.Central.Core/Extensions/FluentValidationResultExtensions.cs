using DigitalSkynet.DotnetCore.DataStructures.Exceptions.Api;
using FluentValidation.Results;

namespace Flash.Central.Core.Extensions
{
    /// <summary>
    /// Class. Implements extensions for FluentValidation
    /// </summary>
	public static class FluentValidationResultExtensions
	{
        /// <summary>
        /// Extension. Creates the result of validation
        /// </summary>
        /// <param name="validationResult">ValidationResult by FluentValidation</param>
        /// <returns>Validation's result</returns>
		public static DigitalSkynet.DotnetCore.DataStructures.Validation.ValidationResult ToDsValidationResult(this ValidationResult validationResult)
		{
			var result = new DigitalSkynet.DotnetCore.DataStructures.Validation.ValidationResult();

			foreach(var issue in validationResult.Errors)
			{
				result.AddError(issue.ErrorMessage);
			}

			return result;
		}

        /// <summary>
        /// Extension. Creates an ApiValidationException if not valid
        /// </summary>
        /// <param name="validationResult">ValidationResult by FluentValidation</param>
		public static void ThrowIfNotValid(this ValidationResult validationResult)
		{
			if (!validationResult.IsValid)
				throw new ApiValidationException(validationResult.ToDsValidationResult());
		}
	}
}
