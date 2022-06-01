using DigitalSkynet.DotnetCore.DataStructures.Exceptions.Api;
using Flash.Central.Core.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Flash.Central.Api.Middleware
{
    /// <summary>
    /// Class. Implements validation intercepter
    /// </summary>
    public class ValidatorInterceptor : IValidatorInterceptor
    {

        /// <summary>
        /// A method that is called after ASP.MVC performs its built-in model validation
        /// </summary>
        /// <param name="actionContext">ActionContext</param>
        /// <param name="validationContext">IValidationContext by FluentValidation</param>
        /// <param name="result">ValidationResult by FluentValidation</param>
        /// <returns></returns>
        public ValidationResult AfterAspNetValidation (ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
        {
            if (!result.IsValid)
                throw new ApiValidationException (result.ToDsValidationResult ());

            return result;
        }

        /// <summary>
        /// A method that is called before ASP.NET's built-in validations
        /// </summary>
        /// <param name="actionContext">ActionContext</param>
        /// <param name="commonContext">IValidationContext by FluentValidation</param>
        /// <returns></returns>
        public IValidationContext BeforeAspNetValidation (ActionContext actionContext, IValidationContext commonContext)
        {
            return commonContext;
        }
    }
}
