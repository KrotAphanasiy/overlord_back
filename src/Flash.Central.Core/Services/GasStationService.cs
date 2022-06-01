using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitalSkynet.DotnetCore.DataAccess.Enums;
using DigitalSkynet.DotnetCore.DataAccess.UnitOfWork;
using Flash.Central.Core.Services.Base;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.Core.Validation.Interface;
using Flash.Central.Data.Repositories.Interfaces;
using Flash.Central.Dtos.GasStation;
using Flash.Central.Foundation.Base.Models;
using Flash.Central.Foundation.Enums;
using Flash.Central.Foundation.Pagination;
using Flash.Central.ViewModel.GasStation;
using Flash.Domain.Entities;

namespace Flash.Central.Core.Services
{
    /// <summary>
    /// Class. Implements contract for all members of IGasStationService.
    /// Derived from ValidationServiceBase.
    /// </summary>
	public class GasStationService : ValidationServiceBase<GasStationModel, IGasStationRepository, GasStation, long>, IGasStationService
	{
		protected override string EntityName => "Gas Station";
        private readonly ICameraRepository _cameraRepository;
        private readonly ITerminalRepository _terminalRepository;

        /// <summary>
        /// Gets paged gas station objects by predicate. Overriden
        /// </summary>
        /// <typeparam name="TProjection"></typeparam>
        /// <param name="predicate">Linq expression</param>
        /// <param name="pagination">The object of PaginationModel to enumerate and sort pages</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection of gas stations and page info</returns>
        protected override async Task<Paged<TProjection>> GetPagedByPredicate<TProjection>(Expression<Func<GasStation, bool>> predicate, PaginationModel pagination, CancellationToken ct)
        {
            var count = await _repository.CountAsync(predicate, ct);
            var dtos = await _repository.ProjectToAsync<TProjection> (predicate,
                args : new PaginationArgs
                {
                    PageSize = pagination.PageSize,
                    PageNumber = pagination.PageNumber,
                    Sortings = pagination.Sortings ?? new List<SortModel>
                    {
                        new SortModel
                        {
                            FieldName = "Name",
                            Direction = SortDirections.Asc
                        }
                    }
                }, FetchModes.NoTracking, ct : ct);
            return new Paged<TProjection> (dtos, count, pagination.PageNumber, pagination.PageSize);
        }

        /// <summary>
        /// Constructor. Initializes parameters
        /// </summary>
        /// <param name="cameraRepository">Defines ICameraRepository</param>
        /// <param name="terminalRepository">Defines ITerminalRepository</param>
        /// <param name="mapper">Defines AutoMapper methods and properties</param>
        /// <param name="unitOfWork">Defines IUnitOfWork methods</param>
        /// <param name="repository">Defines IGasStationRepository</param>
        /// <param name="validator">Defines IGasStationValidator</param>
        public GasStationService(
            ICameraRepository cameraRepository,
            ITerminalRepository terminalRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IGasStationRepository repository,
            IGasStationValidator validator) :
            base(mapper, unitOfWork, repository, validator)
        {
            _cameraRepository = cameraRepository;
            _terminalRepository = terminalRepository;
        }

        /// <summary>
        /// Creates gas station
        /// </summary>
        /// <param name="model">The object of GasStationModel
        /// <see cref="GasStationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Newly created gas station object</returns>
		public async Task<GasStationVm> Create(GasStationModel model, CancellationToken ct)
		{
			await ValidateInputModel(model, ct);
			var newEntity = _repository.Create(model);
			await _unitOfWork.SaveChangesAsync(ct);
			return await Get(newEntity.Id, ct);
		}

        /// <summary>
        /// Deletes a gas station by id if neither camera nor terminal are bound to it
        /// </summary>
        /// <param name="id">Gas station's id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Boolean value</returns>
		public async Task<bool> Delete(long id, CancellationToken ct)
        {
            var gasStationCamera = await _cameraRepository.FindFirstAsync(
                x => x.GasStationId == id, ct: ct);
            if (gasStationCamera != null) return false;

            var gasStationTerminal = await _terminalRepository.FindFirstAsync(
                x => x.GasStationId == id, ct: ct);
            if (gasStationTerminal != null) return false;

            await _repository.Delete(id, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            var ifExists = await _repository.ExistsAsync(id, ct: ct);
            return !ifExists;
        }

        /// <summary>
        /// Gets a gas station by id
        /// </summary>
        /// <param name="id">Gas station's id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Gas station's object</returns>
		public async Task<GasStationVm> Get(long id, CancellationToken ct)
		{
			var dto = await _repository.GetProjectionByIdAsync<GasStationDto>(id, ct: ct);
			ValidateModelExists(dto);
			var result = _mapper.Map<GasStationVm>(dto);
			return result;
		}

        /// <summary>
        /// Gets all gas stations
        /// </summary>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection of gas stations</returns>
		public async Task<List<GasStationVm>> GetAll(CancellationToken ct)
		{
			var dtos = await _repository.ProjectToAsync<GasStationDto>(x => true, ct: ct);
			var result = _mapper.Map<List<GasStationVm>>(dtos);
			return result;
		}

        /// <summary>
        /// Gets paged collection of gas stations
        /// </summary>
        /// <param name="pagination">The object of PaginationModel to enumerate and sort  pages
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection of gas stations</returns>
        public async Task<Paged<GasStationVm>> GetPagedAsync(PaginationModel pagination, CancellationToken ct)
        {
            Expression<Func<GasStation, bool>> predicate = x => pagination.FilterString == null || x.Id.ToString ().Contains (pagination.FilterString);
            var paged = await GetPagedByPredicate<GasStationDto> (predicate, pagination, ct);
            var vms = _mapper.Map<List<GasStationVm>> (paged.Data);
            var result = new Paged<GasStationVm> (vms, paged.Total, paged.PageNumber, paged.PageSize);
            return result;
        }

        /// <summary>
        /// Updates gas station by id
        /// </summary>
        /// <param name="id">Gas station's id</param>
        /// <param name="model">The object of GasStationModel
        /// <see cref="GasStationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Updated gas station object</returns>
		public async Task<GasStationVm> Update(long id, GasStationModel model, CancellationToken ct)
        {
            await ValidateInputModel(model, ct);
            await _repository.UpdateAsync(id, model, ct: ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return await Get(id, ct);
		}
	}
}
