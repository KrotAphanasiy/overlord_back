using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitalSkynet.DotnetCore.DataAccess.Repository;
using DigitalSkynet.DotnetCore.DataAccess.UnitOfWork;
using Flash.Central.Core.Extensions;
using Flash.Central.Foundation.Base.Entities;
using FluentValidation;

namespace Flash.Central.Core.Services.Base
{
	/// <summary>
	/// Class. Represents validation service base. Has protected validator required.
	/// Inherits base entity service.
	/// </summary>
	/// <typeparam name="TInputModel"></typeparam>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	public abstract class ValidationServiceBase<TInputModel, TRepository, TEntity, TKey> : KeyEntityServiceBase<TRepository, TEntity, TKey>
		where TRepository: IGenericRepository<TEntity, TKey>
		where TEntity : BaseKeyEntity<TKey>
		where TKey : struct
	{
		protected readonly IValidator<TInputModel> _validator;
		// TODO Move to nuget
		protected ValidationServiceBase(IMapper mapper,
			IUnitOfWork unitOfWork,
			TRepository repository,
			IValidator<TInputModel> validator) : base(mapper, unitOfWork, repository)
		{
			_validator = validator;
		}

        /// <summary>
        /// Validates input model
        /// </summary>
        /// <param name="model">Generic model</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>The result of validation</returns>
        protected async Task ValidateInputModel(TInputModel model, CancellationToken ct)
		{
			var validationResult = await _validator.ValidateAsync(model, ct);
			validationResult.ThrowIfNotValid();
		}
	}
}
