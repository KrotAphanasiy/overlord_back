using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitalSkynet.DotnetCore.DataAccess.UnitOfWork;
using Flash.Central.Core.Services.Base;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.Core.Validation.Interface;
using Flash.Central.Data.Repositories.Interfaces;
using Flash.Central.Dtos.Camera;
using Flash.Central.Dtos.DetectionEvent;
using Flash.Central.Dtos.GasStation;
using Flash.Central.Dtos.RecognitionEvent;
using Flash.Central.ViewModel.Camera;
using Flash.Domain.Entities;


namespace Flash.Central.Core.Services
{
    /// <summary>
    /// Class. Implements contract for all members of ICameraService.
    /// Derived from ValidationServiceBase.
    /// </summary>
    public class CameraService : ValidationServiceBase<CameraModel, ICameraRepository, Camera, Guid>, ICameraService
	{
        private readonly IRecognitionEventRepository _recognitionEventRepository;
        private readonly IDetectionEventRepository _detectionEventRepository;
        private readonly ITerminalRepository _terminalRepository;
        private readonly ICameraRegionRepository _cameraRegionRepository;

		protected override string EntityName => "Camera";

        /// <summary>
        /// Constructor. Initializes parameters
        /// </summary>
        /// <param name="repository">Defines ICameraRepository</param>
        /// <param name="recognitionEventRepository">Defines IRecognitionEventRepository</param>
        /// <param name="detectionEventRepository">Defines IDetectionEventRepository</param>
        /// <param name="terminalRepository">Defines ITerminalRepository</param>
        /// <param name="validator">Defines ICameraValidator</param>
        /// <param name="mapper">Defines AutoMapper methods and properties</param>
        /// <param name="unitOfWork">Defines IUnitOfWork methods</param>
		public CameraService(ICameraRepository repository,
            IRecognitionEventRepository recognitionEventRepository,
            IDetectionEventRepository detectionEventRepository,
            ICameraRegionRepository cameraRegionRepository,
            ITerminalRepository terminalRepository,
            ICameraValidator validator,
			IMapper mapper,
			IUnitOfWork unitOfWork) : base(mapper, unitOfWork, repository, validator)
		{
            _recognitionEventRepository = recognitionEventRepository;
            _detectionEventRepository = detectionEventRepository;
            _terminalRepository = terminalRepository;
            _cameraRegionRepository = cameraRegionRepository;
        }

        /// <summary>
        /// Gets all cameras.
        /// </summary>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Camera's collection</returns>
		public async Task<List<CameraVm>> GetAll(CancellationToken ct)
		{
			var dtos = await _repository.ProjectToAsync<CameraDto>(x => true, ct: ct);
			var result = _mapper.Map<List<CameraVm>>(dtos);
			return result;
		}

        /// <summary>
        /// Gets exact camera by guid.
        /// </summary>
        /// <param name="id">Camera's guid</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Camera's object</returns>
		public async Task<CameraVm> Get(Guid id, CancellationToken ct)
		{
			var dto = await _repository.GetProjectionByIdAsync<CameraDto>(id, ct: ct);
			ValidateModelExists(dto);
			var result = _mapper.Map<CameraVm>(dto);
			return result;
		}


        /// <summary>
        /// Creates a camera.
        /// </summary>
        /// <param name="model">The object of CameraModel</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Newly created camera's object</returns>
        public async Task<CameraVm> Create(CameraModel model, CancellationToken ct)
		{
			await ValidateInputModel(model, ct);

			var newCamera = _repository.Create(model);
			await _unitOfWork.SaveChangesAsync(ct);

			return await Get(newCamera.Id, ct);
		}

        /// <summary>
        /// Updates camera by guid.
        /// </summary>
        /// <param name="id">Camera's guid</param>
        /// <param name="model">The object of CameraModel</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Updated camera's object</returns>
		public async Task<CameraVm> Update(Guid id, CameraModel model, CancellationToken ct)
        {
            await ValidateInputModel(model, ct);
            await _repository.UpdateAsync(id, model, ct: ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return await Get(id, ct);
        }

        /// <summary>
        /// Deletes camera by id if camera doesn't have any recognition or detection event and there is no
        /// assigned camera's region to terminal
        /// </summary>
        /// <param name="id">Camera's guid</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns></returns>
		public async Task<bool> Delete(Guid id, CancellationToken ct)
        {
            var cameraRecognitionEvent =
                await _recognitionEventRepository.FindFirstAsync(x => x.CameraRegion.CameraId == id, ct: ct);
            if (cameraRecognitionEvent != null) return false;

            var cameraDetectionEvent =
                await _detectionEventRepository.FindFirstAsync(x => x.CameraRegion.CameraId == id, ct: ct);
            if (cameraDetectionEvent != null) return false;

            var cameraRegionsWithTerminals =
                await _cameraRegionRepository.FindFirstAsync(x => (x.CameraId == id) && (x.TerminalId != null), ct: ct);
            if (cameraRegionsWithTerminals!= null) return false;

            var regionsToHide = await _cameraRegionRepository.ProjectToAsync<CameraRegionDto>(
                x => x.CameraId == id);

            foreach (var region in regionsToHide)
            {
                await _cameraRegionRepository.Delete(region.Id, ct);
            }

            await _repository.Delete(id, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            var isExist = await _repository.ExistsAsync (id, ct: ct);
            return !isExist;
		}

        /// <summary>
        /// Detects if there is any authoriztion api key for this camera by camera's guid
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="apiKey">Camera's api key</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Boolean value</returns>
		public async Task<bool> IsMatchApiKey(Guid uid, string apiKey, CancellationToken ct)
		{
			var cameraDto = await _repository.GetProjectionByIdAsync<CameraDto>(uid, ct: ct);
			var isMatch = false;
			if (cameraDto != null)
			{
				isMatch = cameraDto.ApiKey == apiKey;
			}

			return isMatch;
		}

        /// <summary>
        /// Gets cameras for concrete gas station by its id
        /// </summary>
        /// <param name="gasStationId">Gas station's id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns></returns>
        public async Task<List<CameraVm>> GetForStation(long gasStationId, CancellationToken ct = default)
        {
            var dto = await _repository.ProjectToAsync<CameraDto>(x => x.GasStationId == gasStationId, ct: ct);
            ValidateModelExists(dto);
            var result = _mapper.Map<List<CameraVm>>(dto);
            return result;
        }
    }
}
