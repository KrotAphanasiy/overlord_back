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
using Flash.Central.Dtos.RecognitionEvent;
using Flash.Central.Foundation.Enums;
using Flash.Central.Core.Extensions;
using Flash.Central.Foundation.Base.Models;
using Flash.Central.Foundation.Pagination;
using Flash.Central.ViewModel.DetectionEvent;
using Flash.Central.ViewModel.RecognitionEvent;
using Flash.Domain.Entities;

namespace Flash.Central.Core.Services
{
    /// <summary>
    /// Class. Implements contract for all members of IRecognitionEventService.
    /// Derived from ValidationServiceBase.
    /// </summary>
	public class RecognitionEventService : ValidationServiceBase<RecognitionEventModel, IRecognitionEventRepository, RecognitionEvent, Guid>, IRecognitionEventService
	{
        private readonly IPictureService _pictureService;
        private readonly IVisitRepository _visitRepository;
        private readonly IDetectionEventRepository _detectionEventRepository;

		protected override string EntityName => "Recognition Event";

        /// <summary>
        /// Gets recognition events by predicate. Overriden
        /// </summary>
        /// <typeparam name="TProjection"></typeparam>
        /// <param name="predicate">Linq expression</param>
        /// <param name="pagination">The object of PaginationModel to enumerate and sort  pages</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection of recognition events</returns>
		protected override async Task<Paged<TProjection>> GetPagedByPredicate<TProjection> (Expression<Func<RecognitionEvent, bool>> predicate, PaginationModel pagination, CancellationToken ct)
		{
			var count = await _repository.CountAsync (predicate, ct : ct);
			var dtos = await _repository.ProjectToAsync<TProjection> (predicate,
                new PaginationArgs
                {
                    PageSize = pagination.PageSize,
                    PageNumber = pagination.PageNumber,
                    Sortings = pagination.Sortings ??
                               new List<SortModel>
                               {
                                   new SortModel
                                   {
                                       FieldName = "Timestamp",
                                       Direction = SortDirections.Desc
                                   }
                               }
                }, FetchModes.NoTracking, ct : ct);
			return new Paged<TProjection> (dtos, count, pagination.PageNumber, pagination.PageSize);
		}

        /// <summary>
        /// Constructor. Initializes parameters
        /// </summary>
        /// <param name="repository">Defines IRecognitionEventRepository</param>
        /// <param name="detectionEventRepository">Defines IDetectionEventRepository</param>
        /// <param name="visitRepository">Defines IVisitRepository</param>
        /// <param name="pictureService">Defines IPictureService</param>
        /// <param name="mapper">Defines AutoMapper methods and properties</param>
        /// <param name="validator">Defines IRecognitionEventValidator</param>
        /// <param name="unitOfWork">Defines IUnitOfWork methods</param>
		public RecognitionEventService (IRecognitionEventRepository repository,
            IDetectionEventRepository detectionEventRepository,
            IVisitRepository visitRepository,
            IPictureService pictureService,
			IMapper mapper,
			IRecognitionEventValidator validator,
			IUnitOfWork unitOfWork) : base (mapper, unitOfWork, repository, validator)
		{
            _pictureService = pictureService;
            _visitRepository = visitRepository;
            _detectionEventRepository = detectionEventRepository;
        }

        /// <summary>
        /// Creates recogntion event and updates bound detection event
        /// </summary>
        /// <param name="model">The object of RecognitionEventModel
        /// <see cref="RecognitionEventModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Created recognition event</returns>
		public async Task<RecognitionEventVm> Create (RecognitionEventModel model, CancellationToken ct)
		{
			var validationResult = await _validator.ValidateAsync(model, ct);
			validationResult.ThrowIfNotValid();

			var entity = _mapper.Map<RecognitionEvent>(model);

			var image = Convert.FromBase64String(model.ImageBase64);
			var imageLink = await _pictureService.Save(image, ct);
			if (!string.IsNullOrEmpty(model.ProcessedImageBase64))
			{
				var processedImage = Convert.FromBase64String(model.ProcessedImageBase64);
				var processedImageLink = await _pictureService.Save(processedImage, ct);
				entity.ProcessedImageLink = processedImageLink;
			}

			entity.ImageLink = imageLink;
			_repository.Create (entity);

            var detectionEventToUpdate = await _detectionEventRepository.GetByIdAsync(
                model.DetectionEventId, ct: ct);

            if (detectionEventToUpdate != null)
            {
                entity.ImageLink = detectionEventToUpdate.OriginalImageLink;
                entity.ProcessedImageLink = detectionEventToUpdate.CroppedImageLink;

                _repository.Create(entity);

                await _detectionEventRepository.UpdateAsync(
                    model.DetectionEventId,
                    new DetectionEventModel
                    {
                        CameraRegionId = detectionEventToUpdate.CameraRegionId,
                        Timestamp = detectionEventToUpdate.Timestamp,
                        Probability = detectionEventToUpdate.Probability,
                        Processed = true
                    },
                    ct: ct
                );
            }

            await _unitOfWork.SaveChangesAsync(ct);

			return await Get(entity.Id, ct);
		}

        /// <summary>
        /// Deletes recogntion event by id.
        /// </summary>
        /// <param name="id">Recognition event's id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Boolean value</returns>
		public async Task<bool> Delete(Guid id, CancellationToken ct)
		{
			await _repository.DeleteHardAsync(id, ct: ct);
            await _unitOfWork.SaveChangesAsync(ct);
			var isExist = await _repository.ExistsAsync(id, ct: ct);
			return !isExist;
		}

        /// <summary>
        /// Gets recognition event by id
        /// </summary>
        /// <param name="id">Recognition event's id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Recognition event's object</returns>
		public async Task<RecognitionEventVm> Get (Guid id, CancellationToken ct)
		{

			var dto = await _repository.GetProjectionByIdAsync<RecognitionEventDto> (id, ct : ct);
			ValidateModelExists (dto);
			var result = _mapper.Map<RecognitionEventVm> (dto);
			return result;
		}

        /// <summary>
        /// Gets paged collection of recognition events
        /// </summary>
        /// <param name="pagination">The object of PaginationModel to enumerate and sort  pages
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection of recognition events</returns>
		public async Task<Paged<RecognitionEventVm>> GetPagedAsync (PaginationModel pagination, CancellationToken ct = default)
		{
            Expression<Func<RecognitionEvent, bool>> predicate = x => pagination.FilterString == null || x.CameraRegionId.ToString ().Contains (pagination.FilterString);
			var paged = await GetPagedByPredicate<RecognitionEventDto> (predicate, pagination, ct);
			var vms = _mapper.Map<List<RecognitionEventVm>> (paged.Data);
			var result = new Paged<RecognitionEventVm> (vms, paged.Total, paged.PageNumber, paged.PageSize);
			return result;
		}

        /// <summary>
        /// Gets paged collection of recognition events by camera's guid. Can be filtered by dates
        /// </summary>
        /// <param name="pagination">The object of PaginationModel to enumerate and sort pages
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="cameraUid">Camera's guid</param>
        /// <param name="startedAt">Optional. The date of event's start</param>
        /// <param name="endedAt">Optional. The date of event's end</param>
        /// <param name="ct"></param>
        /// <returns>Paged collection of recognition events</returns>
		public async Task<Paged<RecognitionEventVm>> GetPagedByCameraUidAsync(PaginationModel pagination, Guid cameraUid, DateTime? startedAt,
            DateTime? endedAt, CancellationToken ct)
		{
            Expression<Func<RecognitionEvent, bool>> predicate = x => (pagination.FilterString == null || x.CameraRegionId.ToString().Contains(pagination.FilterString)) &&
                                                                      x.CameraRegion.CameraId == cameraUid && x.Timestamp >= (startedAt ?? DateTime.MinValue) && (x.Timestamp <= (endedAt ?? DateTime.UtcNow));

            var paged = await GetPagedByPredicate<RecognitionEventDto> (predicate, pagination, ct);
			var vms = _mapper.Map<List<RecognitionEventVm>> (paged.Data);
			var result = new Paged<RecognitionEventVm> (vms, paged.Total, paged.PageNumber, paged.PageSize);
			return result;
		}

        /// <summary>
        /// Gets collection of recognition events by camera guid between certain dates
        /// </summary>
        /// <param name="cameraId">Optional. Camera's guid</param>
        /// <param name="startedAt">The date of event's start</param>
        /// <param name="endedAt">The date of event's end</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection of recognition events</returns>
        public async Task<List<RecognitionEventVm>> GetBetweenDates(Guid? cameraId, DateTime startedAt, DateTime endedAt, CancellationToken ct)
        {
            Expression<Func<RecognitionEvent, bool>> predicate;
            if (cameraId != null)
                predicate = x => x.Timestamp >= startedAt && x.Timestamp <= endedAt
                && x.CameraRegion.CameraId == cameraId;
            else
                predicate = x => x.Timestamp >= startedAt && x.Timestamp <= endedAt;

            var dtos = await _repository.ProjectToAsync<RecognitionEventDto>(predicate, ct: ct);
            var result = _mapper.Map<List<RecognitionEventVm>>(dtos);
            return result;
        }

        /// <summary>
        /// Checks if image exists
        /// </summary>
        /// <param name="imageLink">Link to image</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Boolean value</returns>
        public async Task<bool> ImageExists(string imageLink, CancellationToken ct = default)
        {
            var dtos = await _repository.ProjectToAsync<RecognitionEventDto>(
                x => x.ProcessedImageLink == imageLink, ct: ct);
            return dtos.Any();
        }

        /// <summary>
        /// Gets list of image links to delete and responding visit ids and a link to image to replace deleted ones
        /// </summary>
        /// <param name="picturesToKeep">Number of pictures</param>
        /// <param name="timestamp">Number of pictures</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection</returns>
        public async Task<List<(string, long, string)>> GetImageLinksFromProcessedEvents(int picturesToKeep, DateTime timestamp, CancellationToken ct = default)
        {
            var dtos = await _repository.ProjectToAsync<RecognitionEventDto>(
                x=>x.VisitId != null && !x.Visit.IsClean && x.Timestamp < timestamp);

            var visitIds = dtos.Select(x => x.VisitId).ToList();
            var visits = await _visitRepository.GetByIdsAsync(visitIds);

            var eventIdsToSave = new List<Guid>();
            foreach (var visit in visits)
            {
                eventIdsToSave = eventIdsToSave.Concat(dtos.Where(dto => dto.PlateNumber == visit.PlateNumber).Select(dto => dto.Id).Take(picturesToKeep)).ToList();
            }

            dtos = dtos.Where(dto => !eventIdsToSave.Contains(dto.Id)).ToList();

            var replacingLink = dtos.FirstOrDefault(dto => eventIdsToSave.Contains(dto.Id))?.ImageLink;

            var result = from recognitionEventDto in dtos
                select (recognitionEventDto.ImageLink, recognitionEventDto.VisitId, replacingLink);

            return result.ToList();
        }

        /// <summary>
        /// Gets list of image links to delete and responding visit ids and a link to image to replace deleted ones
        /// </summary>
        /// <param name="picturesToKeep">Number of pictures</param>
        /// <param name="timestamp">The date to take before</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection</returns>
        public async Task<List<(string, long, string)>> GetProcessedImageLinksFromProcessedEvents(int picturesToKeep, DateTime timestamp, CancellationToken ct = default)
        {
            var dtos = await _repository.ProjectToAsync<RecognitionEventDto>(
                x=>x.VisitId != null && !x.Visit.IsClean && x.Timestamp < timestamp);

            var visitIds = dtos.Select(x => x.VisitId).ToList();
            var visits = await _visitRepository.GetByIdsAsync(visitIds);

            var eventIdsToSave = new List<Guid>();
            foreach (var visit in visits)
            {
                eventIdsToSave = eventIdsToSave.Concat(dtos.Where(dto => dto.PlateNumber == visit.PlateNumber).Select(dto => dto.Id).Take(picturesToKeep)).ToList();
            }

            dtos = dtos.Where(dto => !eventIdsToSave.Contains(dto.Id)).ToList();

            var replacingLink = dtos.FirstOrDefault(dto => eventIdsToSave.Contains(dto.Id))?.ProcessedImageLink;

            var result = from recognitionEventDto in dtos
                select (recognitionEventDto.ProcessedImageLink, recognitionEventDto.VisitId, replacingLink);

            return result.ToList();
        }

        /// <summary>
        /// Gets all recognition events
        /// </summary>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection of recognition events</returns>
        public async Task<List<RecognitionEventVm>> GetAll (CancellationToken ct = default)
		{
            var dtos = await _repository.ProjectToAsync<RecognitionEventDto> (x => true, ct : ct);
			var result = _mapper.Map<List<RecognitionEventVm>> (dtos);
			return result;
		}

        /// <summary>
        /// Updates recognition event by id
        /// </summary>
        /// <param name="id">Recognition event id</param>
        /// <param name="model">The object of RecognitionEventModel
        /// <see cref="RecognitionEventModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Updated recognition event</returns>
		public async Task<RecognitionEventVm> Update (Guid id, RecognitionEventModel model, CancellationToken ct)
		{
            var validationResult = await _validator.ValidateAsync (model, ct);
			validationResult.ThrowIfNotValid ();
			var entity = _mapper.Map<RecognitionEvent> (model);
			await _repository.UpdateAsync (id, entity);
            await _unitOfWork.SaveChangesAsync(ct);
            return await Get (entity.Id, ct);
		}
    }
}
