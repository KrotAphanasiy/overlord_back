using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitalSkynet.DotnetCore.DataAccess.Enums;
using DigitalSkynet.DotnetCore.DataAccess.UnitOfWork;
using Flash.Central.Core.Extensions;
using Flash.Central.Core.Services.Base;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.Data.Repositories.Interfaces;
using Flash.Central.Dtos.DetectionEvent;
using Flash.Central.Dtos.MqDtos.Messages;
using Flash.Central.Foundation.Base.Models;
using Flash.Central.Foundation.Enums;
using Flash.Central.Foundation.Pagination;
using Flash.Central.ViewModel.DetectionEvent;
using Flash.Domain.Entities;
using FluentValidation;


namespace Flash.Central.Core.Services
{
    /// <summary>
    /// Class. Implements contract for all members of IDetectionEventService.
    /// Derived from ValidationServiceBase.
    /// </summary>
    public class DetectionEventService : ValidationServiceBase<DetectionEventModel, IDetectionEventRepository, DetectionEvent, Guid>, IDetectionEventService
    {
        private readonly IQueueService<DetectionEventMessage> _queueService;
        private readonly IPictureService _pictureService;

        protected override string EntityName => "Detection Event";

        /// <summary>
        /// Gets detection events by predicate. Overriden.
        /// </summary>
        /// <typeparam name="TProjection"></typeparam>
        /// <param name="predicate">Linq expression</param>
        /// <param name="pagination">The object of PaginationModel to enumerate  pages
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection and page's information</returns>
        protected override async Task<Paged<TProjection>> GetPagedByPredicate<TProjection> (Expression<Func<DetectionEvent, bool>> predicate, PaginationModel pagination, CancellationToken ct)
        {
            var count = await _repository.CountAsync (predicate, ct : ct);
            var dtos = await _repository.ProjectToAsync<TProjection> (predicate, args : new PaginationArgs { PageSize = pagination.PageSize, PageNumber = pagination.PageNumber, Sortings = pagination.Sortings }, FetchModes.NoTracking, ct : ct);
            return new Paged<TProjection> (dtos, count, pagination.PageNumber, pagination.PageSize);
        }

        /// <summary>
        /// Constructor. Initializes parameters
        /// </summary>
        /// <param name="queueService">Defines IQueueService</param>
        /// <param name="pictureService">Defines IPictureService</param>
        /// <param name="mapper">Defines AutoMapper methods and properties</param>
        /// <param name="unitOfWork">Defines IUnitOfWork methods</param>
        /// <param name="repository">Defines IDetectionEventRepository</param>
        /// <param name="validator">Defines IValidator with DetectionEventModel as generic type</param>
        public DetectionEventService(IQueueService<DetectionEventMessage> queueService, IPictureService pictureService,
            IMapper mapper, IUnitOfWork unitOfWork, IDetectionEventRepository repository, IValidator<DetectionEventModel> validator)
            : base(mapper, unitOfWork, repository, validator)
        {
            _queueService = queueService;
            _pictureService = pictureService;
        }

        /// <summary>
        /// Gets all paged detection events
        /// </summary>
        /// <param name="pagination">The object of PaginationModel to enumerate  pages
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection of detection events</returns>
        public async Task<Paged<DetectionEventVm>> GetPagedAsync(PaginationModel pagination, CancellationToken ct)
        {
            pagination.Sortings ??= new List<SortModel> { new SortModel { FieldName = "Timestamp", Direction = SortDirections.Desc } };
            Expression<Func<DetectionEvent, bool>> predicate = x => pagination.FilterString == null || x.CameraRegionId.ToString ().Contains (pagination.FilterString);
            var paged = await GetPagedByPredicate<DetectionEventDto> (predicate, pagination, ct);
            var vms = _mapper.Map<List<DetectionEventVm>> (paged.Data);
            var result = new Paged<DetectionEventVm> (vms, paged.Total, paged.PageNumber, paged.PageSize);
            return result;
        }

        /// <summary>
        /// Gets events between concrete dates
        /// </summary>
        /// <param name="startedAt">The date of event's start</param>
        /// <param name="endedAt">The date of event's end</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection of events</returns>
        public async Task<List<DetectionEventVm>> GetBetweenDates(DateTime startedAt, DateTime endedAt, CancellationToken ct)
        {
            Expression<Func<DetectionEvent, bool>> predicate = x => x.CreatedDate >= startedAt && x.CreatedDate <= endedAt;
            var dtos = await _repository.ProjectToAsync<DetectionEventDto> (predicate, ct : ct);
            var result = _mapper.Map<List<DetectionEventVm>> (dtos);
            return result;
        }

        /// <summary>
        /// Gets all detection events
        /// </summary>
        /// <param name="ct"></param>
        /// <returns>Collection of detection events</returns>
        public async Task<List<DetectionEventVm>> GetAll(CancellationToken ct)
        {
            var dtos = await _repository.ProjectToAsync<DetectionEventDto>(x => true, ct: ct);
            return _mapper.Map<List<DetectionEventVm>>(dtos);
        }

        /// <summary>
        /// Gets detection event by guid
        /// </summary>
        /// <param name="id">Detection event's guid</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>The detection event's object</returns>
        public async Task<DetectionEventVm> Get(Guid id, CancellationToken ct)
        {
            var dto = await _repository.GetProjectionByIdAsync<DetectionEventDto>(id, ct: ct);
            ValidateModelExists(dto);
            return _mapper.Map<DetectionEventVm>(dto);
        }

        /// <summary>
        /// Checks if image exists
        /// </summary>
        /// <param name="imageLink">Link to the image in storage</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Boolean value</returns>
        public async Task<bool> ImageExists(string imageLink, CancellationToken ct = default)
        {
            var dtos = await _repository.ProjectToAsync<DetectionEventDto>(
                x => x.OriginalImageLink == imageLink || x.CroppedImageLink == imageLink, ct: ct);
            return dtos.Any();
        }

        /// <summary>
        /// Gets original image's links before certain date
        /// </summary>
        /// <param name="picturesToKeep">number of pictures</param>
        /// <param name="timestamp">The date to get images before</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>The collection of links</returns>
        public async Task<List<string>> GetOriginalImageLinksBeforeTimestamp(int picturesToKeep, DateTime timestamp, CancellationToken ct = default)
        {
            var dtos = await _repository.ProjectToAsync<DetectionEventDto>(
                x=>x.Timestamp < timestamp);

            var eventsToSave = dtos.Select(x => x.Id).Take(picturesToKeep);

            var result = dtos.Where(dto => !eventsToSave.Contains(dto.Id)).Select(dto => dto.OriginalImageLink).ToList();

            return result;
        }

        /// <summary>
        /// Gets cropped image's links before certain date
        /// </summary>
        /// <param name="picturesToKeep">number of pictures</param>
        /// <param name="timestamp">The date to get images before</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>The collection of links</returns>
        public async Task<List<string>> GetCroppedImageLinksBeforeTimestamp(int picturesToKeep, DateTime timestamp, CancellationToken ct = default)
        {
            var dtos = await _repository.ProjectToAsync<DetectionEventDto>(
                x=>x.Timestamp < timestamp);

            var eventsToSave = dtos.Select(x => x.Id).Take(picturesToKeep);

            var result = dtos.Where(dto => !eventsToSave.Contains(dto.Id)).Select(dto => dto.CroppedImageLink).ToList();

            return result;
        }

        /// <summary>
        /// Creates detection event and enqueues it.
        /// </summary>
        /// <param name="model">The object of DetectionEventModel
        /// <see cref="DetectionEventModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Created detection event</returns>
        public async Task<DetectionEventVm> Create(DetectionEventModel model, CancellationToken ct)
        {
            var validationResult = await _validator.ValidateAsync(model, ct);
            validationResult.ThrowIfNotValid();

            var entity = _mapper.Map<DetectionEvent>(model);

            var originalImage = Convert.FromBase64String(model.OriginalImageBase64);
            entity.OriginalImageLink = await _pictureService.Save(originalImage, ct);

            var croppedImage = Convert.FromBase64String(model.CroppedImageBase64);
            entity.CroppedImageLink = await _pictureService.Save(croppedImage, ct);

            _repository.Create(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            var createdDetectionEvent = await Get(entity.Id, ct);

            await _queueService.Enqueue(new DetectionEventMessage
            {
                CameraRegionId = model.CameraRegionId,
                Timestamp = model.Timestamp,
                Probability = model.Probability,
                OriginalImageBase64 = model.OriginalImageBase64,
                CroppedImageBase64 = model.CroppedImageBase64,
                DetectionEventId = createdDetectionEvent.Id
            }, ct);

            return createdDetectionEvent;
        }

        /// <summary>
        /// Updates detection event by id
        /// </summary>
        /// <param name="id">Detection event id</param>
        /// <param name="model">The object of DetectionEventModel
        /// <see cref="DetectionEventModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Updated detection event's object</returns>
        public async Task<DetectionEventVm> Update(Guid id, DetectionEventModel model, CancellationToken ct)
        {
            await ValidateInputModel(model, ct);
            await _repository.UpdateAsync(id, model, ct: ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return await Get(id, ct);
        }

        /// <summary>
        /// Deletes detection event by id
        /// </summary>
        /// <param name="id">Detection event's id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Boolean value</returns>
        public async Task<bool> Delete(Guid id, CancellationToken ct)
        {
            await _repository.DeleteHardAsync(id, ct: ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return await _repository.ExistsAsync (id, ct: ct);
        }
    }
}
