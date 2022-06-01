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
using Flash.Central.Dtos.Camera;
using Flash.Central.Dtos.GasStation;
using Flash.Central.Foundation.Base.Models;
using Flash.Central.Foundation.Enums;
using Flash.Central.Foundation.Pagination;
using Flash.Central.ViewModel.Camera;
using Flash.Central.ViewModel.GasStation;
using Flash.Domain.Entities;

namespace Flash.Central.Core.Services
{
    /// <summary>
    /// Class. Implements contract for all members of ITerminalService.
    /// Derived from ValidationServiceBase.
    /// </summary>
	public class TerminalService : ValidationServiceBase<TerminalModel, ITerminalRepository, Terminal, long>, ITerminalService
    {
        private readonly ICameraRegionRepository _cameraRegionRepository;
		public TerminalService(
            ICameraRegionRepository cameraRegionRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ITerminalRepository repository,
            ITerminalValidator validator) : base(mapper, unitOfWork, repository, validator)
        {
            _cameraRegionRepository = cameraRegionRepository;
        }
        
		protected override string EntityName => "Terminal";
        /// <summary>
        /// Gets paged collection of terminals by predicate
        /// </summary>
        /// <typeparam name="TProjection"></typeparam>
        /// <param name="predicate">Linq expression</param>
        /// <param name="pagination">The object of PaginationModel to enumerate and sort pages
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection of terminals</returns>
        protected override async Task<Paged<TProjection>> GetPagedByPredicate<TProjection>(Expression<Func<Terminal, bool>> predicate, PaginationModel pagination, CancellationToken ct)
        {
            var count = await _repository.CountAsync(predicate, ct: ct);
            var dtos = await _repository.ProjectToAsync<TProjection>(predicate, args : new PaginationArgs
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
            }, FetchModes.NoTracking, ct: ct);
            return new Paged<TProjection>(dtos, count, pagination.PageNumber, pagination.PageSize);
        }

        /// <summary>
        /// Creates terminal
        /// </summary>
        /// <param name="model">The object of TerminalModel
        /// <see cref="TerminalModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Created terminal's object</returns>
        public async Task<TerminalVm> Create(TerminalModel model, CancellationToken ct)
		{
			await ValidateInputModel(model, ct);
			var newEntity = _repository.Create(model);
			await _unitOfWork.SaveChangesAsync(ct);
			return await Get(newEntity.Id, ct);
		}

        /// <summary>
        /// Deletes terminal by id
        /// </summary>
        /// <param name="id">Terminal's id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Boolean value</returns>
		public async Task<bool> Delete(long id, CancellationToken ct)
		{
            await _repository.Delete(id, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            var regionToUnbind = (await _cameraRegionRepository.ProjectToAsync<CameraRegionDto>(
                x => x.TerminalId == id, ct: ct)).FirstOrDefault();

            if (regionToUnbind != null)
            {
                await _cameraRegionRepository.UpdateAsync(regionToUnbind.Id, new CameraRegionModel
                {
                    CameraId = regionToUnbind.CameraId,
                    TopLeftX = regionToUnbind.TopLeftX,
                    TopLeftY = regionToUnbind.TopLeftY,
                    BottomRightX = regionToUnbind.BottomRightX,
                    BottomRightY = regionToUnbind.BottomRightY,
                    RegionName = regionToUnbind.RegionName,
                    TerminalId = null
                }, ct: ct);
                await _unitOfWork.SaveChangesAsync(ct);
            }

            var isExist = await _repository.ExistsAsync (id, ct: ct);
            return !isExist;
		}

        /// <summary>
        /// Gets terminal by id
        /// </summary>
        /// <param name="id">Terminal's id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Terminal's object</returns>
		public async Task<TerminalVm> Get(long id, CancellationToken ct)
		{
			var dto = await _repository.GetProjectionByIdAsync<TerminalDto>(id, ct: ct);
			ValidateModelExists(dto);
			var result = _mapper.Map<TerminalVm>(dto);
			return result;
		}

        /// <summary>
        /// Gets all terminals
        /// </summary>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection of terminal's objects</returns>
		public async Task<List<TerminalVm>> GetAll(CancellationToken ct)
		{
			var dtos = await _repository.ProjectToAsync<TerminalDto>(x => true, ct: ct);
			var result = _mapper.Map<List<TerminalVm>>(dtos);
			return result;
		}
        /// <summary>
        /// Updates terminal by id
        /// </summary>
        /// <param name="id">Terminal's id</param>
        /// <param name="model">The object of TerminalModel
        /// <see cref="TerminalModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Updated terminal's object</returns>
		public async Task<TerminalVm> Update(long id, TerminalModel model, CancellationToken ct)
        {
            await ValidateInputModel(model, ct);
            await _repository.UpdateAsync(id, model, ct: ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return await Get(id, ct);
		}

        /// <summary>
        /// Gets paged collection of terminals by gas station id 
        /// </summary>
        /// <param name="gasStationId">Gas station's id</param>
        /// <param name="pagination">The object of PaginationModel to enumerate and sort pages
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection of terminals</returns>
        public async Task<Paged<TerminalVm>> GetPagedForGasStation(long gasStationId, PaginationModel pagination, CancellationToken ct)
        {
            Expression<Func<Terminal, bool>> predicate = x =>
                (pagination.FilterString == null || x.Id.ToString().Contains(pagination.FilterString)) &&
                x.AssignedCameraRegion.Camera.GasStationId == gasStationId;

            var paged = await GetPagedByPredicate<TerminalDto>(predicate, pagination, ct);
            var vms = _mapper.Map<List<TerminalVm>>(paged.Data);

            var result = new Paged<TerminalVm>(vms, paged.Total, paged.PageNumber, paged.PageSize);
            return result;
        }
        /// <summary>
        /// Gets paged collection of terminals with unsigned camera's region by gas station's id
        /// </summary>
        /// <param name="gasStationId">Gas station's id</param>
        /// <param name="pagination">The object of PaginationModel to enumerate and sort pages
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection of terminals</returns>
        public async Task<Paged<TerminalVm>> GetPagedAvailableForGasStation(long gasStationId, PaginationModel pagination, CancellationToken ct)
        {
            Expression<Func<Terminal, bool>> predicate = x =>
                (pagination.FilterString == null || x.Id.ToString().Contains(pagination.FilterString)) &&
                x.GasStationId == gasStationId && x.AssignedCameraRegion == null;

            var dtos = await _repository.GetAvailableAsync<TerminalDto>(predicate, new PaginationArgs
            {
                PageSize = pagination.PageSize,
                PageNumber = pagination.PageNumber,
                Sortings = pagination.Sortings
            }, FetchModes.NoTracking, ct: ct);

            var paged = new Paged<TerminalDto>(dtos, dtos.Count, pagination.PageNumber, pagination.PageSize);
            var vms = _mapper.Map<List<TerminalVm>>(paged.Data);

            var result = new Paged<TerminalVm>(vms, paged.Total, paged.PageNumber, paged.PageSize);
            return result;
        }

        /// <summary>
        /// Gets teminals by gas station's id
        /// </summary>
        /// <param name="gasStationId">Gas station's id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection of terminals</returns>
        public async Task<List<TerminalVm>> GetForGasStation(long gasStationId, CancellationToken ct)
        {
            var dtos = await _repository.ProjectToAsync<TerminalDto>(x=>x.GasStationId == gasStationId, ct: ct);
            var result = _mapper.Map<List<TerminalVm>>(dtos);
            return result;
        }
    }
}
