using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitalSkynet.DotnetCore.DataAccess.Repository;
using DigitalSkynet.DotnetCore.DataAccess.UnitOfWork;
using DigitalSkynet.DotnetCore.DataStructures.Exceptions.Api;
using Flash.Central.Foundation.Base.Entities;
using Flash.Central.Foundation.Pagination;
using Flash.Domain.Entities;

namespace Flash.Central.Core.Services.Base
{
    // TODO Move to nuget
    /// <summary>
    /// Abstract class
    /// </summary>
    /// <typeparam name="TRepository">Generic repositoy</typeparam>
    /// <typeparam name="TEntity">Generic entity</typeparam>
    /// <typeparam name="TKey">Generic key</typeparam>
    public abstract class KeyEntityServiceBase<TRepository, TEntity, TKey>
		where TRepository: IGenericRepository<TEntity, TKey>
		where TEntity : BaseKeyEntity<TKey>
		where TKey : struct
	{

		protected abstract string EntityName { get; }
		protected readonly IUnitOfWork _unitOfWork;
		protected readonly IMapper _mapper;
		protected readonly TRepository _repository;

        /// <summary>
        /// Constructor. Initializes parameters
        /// </summary>
        /// <param name="mapper">Defines AutoMapper methods and properties</param>
        /// <param name="unitOfWork">Defines IUnitOfWork methods</param>
        /// <param name="repository">Generic repository</param>
		protected KeyEntityServiceBase(IMapper mapper,
			IUnitOfWork unitOfWork,
			TRepository repository)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_repository = repository;
		}

        /// <summary>
        /// Validates if model exisits
        /// </summary>
        /// <param name="dto">Object</param>
		protected void ValidateModelExists(object dto)
		{
			if (dto == null)
			{
				throw new ApiNotFoundException($"{EntityName} has not been found");
			}
		}

        /// <summary>
        /// Cets paged collection by predicate
        /// </summary>
        /// <typeparam name="TProjection"></typeparam>
        /// <param name="predicate">Linq expression</param>
        /// <param name="pagination">The object of PaginationModel
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns></returns>
		protected virtual async Task<Paged<TProjection>> GetPagedByPredicate<TProjection>(Expression<Func<TEntity, bool>> predicate,  PaginationModel pagination, CancellationToken ct)
		{
			var dtos = await _repository.ProjectToAsync<TProjection>(predicate, ct: ct);
			return new Paged<TProjection>(dtos, 0, 0, 0);
		}
	}
}
