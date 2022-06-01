using System.Collections.Generic;
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
using Flash.Central.ViewModel.Camera;
using Flash.Domain.Entities;

namespace Flash.Central.Core.Services
{
    /// <summary>
    /// Class. Implements contract for all members of ICameraRegionService.
    /// Derived from ValidationServiceBase.
    /// </summary>
    public class CameraRegionService : ValidationServiceBase<CameraRegionModel, ICameraRegionRepository, CameraRegion, long>, ICameraRegionService
	{
		protected override string EntityName => "Camera Region";

        /// <summary>
        /// Constructor. Initializes parameters
        /// </summary>
        /// <param name="mapper">Defines AutoMapper methods and properties</param>
        /// <param name="unitOfWork">Defines IUnitOfWork methods</param>
        /// <param name="repository">Defines ICameraRegionRepository</param>
        /// <param name="validator">Defines ICameraRegionValidator</param>
		public CameraRegionService(IMapper mapper, IUnitOfWork unitOfWork, ICameraRegionRepository repository, ICameraRegionValidator validator) : base(mapper, unitOfWork, repository, validator)
		{
		}

        /// <summary>
        /// Creates new camera's region
        /// </summary>
        /// <param name="model">The object of CameraRegionModel</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Newly created camera's region object</returns>
		public async Task<CameraRegionVm> Create(CameraRegionModel model, CancellationToken ct)
		{
			await ValidateInputModel(model, ct);
			var newRegion = _repository.Create(model);
			await _unitOfWork.SaveChangesAsync(ct);
			return await Get(newRegion.Id, ct);
		}

        /// <summary>
        /// Deletes camera region by id
        /// </summary>
        /// <param name="id">Camera's region id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Boolean value if object is deleted</returns>
		public async Task<bool> Delete(long id, CancellationToken ct)
		{
            var regionToUnbind = await _repository.FindFirstAsync(
                x => x.Id == id, FetchModes.Tracking, ct);

            ValidateModelExists(regionToUnbind);
            regionToUnbind.TerminalId = null;

            await _repository.Delete(id, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            var isExist = await _repository.ExistsAsync (id, ct: ct);
            return !isExist;
		}

        /// <summary>
        /// Gets camera's region by id
        /// </summary>
        /// <param name="id">Camera's region id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Camera's region object</returns>
		public async Task<CameraRegionVm> Get(long id, CancellationToken ct)
		{
			var dto = await _repository.GetProjectionByIdAsync<CameraRegionDto>(id, ct: ct);
			ValidateModelExists(dto);
			var result = _mapper.Map<CameraRegionVm>(dto);
			return result;
		}

        /// <summary>
        /// Gets all camera's regions
        /// </summary>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Camera's region collection</returns>
		public async Task<List<CameraRegionVm>> GetAll(CancellationToken ct)
		{
			var dtos = await _repository.ProjectToAsync<CameraRegionDto>(x => true, ct: ct);
			var result = _mapper.Map<List<CameraRegionVm>>(dtos);
			return result;
		}

        /// <summary>
        /// Updates camera's region by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model">The object of CameraRegionModel</param>
        /// <param name="ct"></param>
        /// <returns>Updated camera's region object</returns>
		public async Task<CameraRegionVm> Update(long id, CameraRegionModel model, CancellationToken ct)
        {
            await ValidateInputModel(model, ct);
            await _repository.UpdateAsync(id, model, ct: ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return await Get(id, ct);
        }
	}
}
