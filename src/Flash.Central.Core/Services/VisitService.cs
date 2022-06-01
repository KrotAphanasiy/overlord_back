using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitalSkynet.DotnetCore.DataAccess.UnitOfWork;
using Flash.Central.Core.Services.Base;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.Data.Repositories.Interfaces;
using Flash.Central.Dtos.Visit;
using Flash.Central.Core.Extensions;
using Flash.Central.ViewModel.Visit;
using Flash.Domain.Entities;
using System.Linq.Expressions;
using Flash.Central.Foundation.Enums;
using DigitalSkynet.DotnetCore.DataAccess.Enums;
using Flash.Central.Foundation.Base.Models;
using Flash.Central.Foundation.Options;
using Flash.Central.Foundation.Pagination;
using Microsoft.Extensions.Options;


namespace Flash.Central.Core.Services
{
    // TODO: think of inheriting from ValidationServiceBase
    /// <summary>
    /// Class. Implements contract for all members of IVisitService.
    /// Derived from KeyEntityServiceBase.
    /// </summary>
	public class VisitService : KeyEntityServiceBase<IVisitRepository, Visit, long>, IVisitService
	{
        private readonly VisitsGenerationOptions _visitsGenerationOptions;
        private readonly IRecognitionEventRepository _recognitionEventRepository;

        protected override string EntityName => "Visit";

        /// <summary>
        /// Constructor. Initializes parameters
        /// </summary>
        /// <param name="recognitionEventRepository"></param>
        /// <param name="generateVisitsOptions">Parameters of VisitsGenerationOptions
        /// <see cref="VisitsGenerationOptions"/>
        /// </param>
        /// <param name="repository">Defines IVisitRepository</param>
        /// <param name="mapper">Defines AutoMapper methods and properties</param>
        /// <param name="unitOfWork">Defines IUnitOfWork methods</param>
        public VisitService (
            IRecognitionEventRepository recognitionEventRepository,
            IOptions<VisitsGenerationOptions> generateVisitsOptions,
			IVisitRepository repository,
            IMapper mapper,
			IUnitOfWork unitOfWork) : base (mapper, unitOfWork, repository)
        {
            _recognitionEventRepository = recognitionEventRepository;
            _visitsGenerationOptions = generateVisitsOptions.Value;
        }
        /// <summary>
        /// Creates visit
        /// </summary>
        /// <param name="model">The object of VisitDto
        /// <see cref="VisitDto"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Newly created visit</returns>
		public async Task<VisitVm> Create (VisitDto model, CancellationToken ct)
        {
            var newVisit = _repository.Create(model);
            await _unitOfWork.SaveChangesAsync(ct);

            return await Get(newVisit.Id, ct);
        }
        /// <summary>
        /// Creates a number of visits
        /// </summary>
        /// <param name="models">VisitDto's collection</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection of created visits</returns>
        public async Task<IEnumerable<VisitVm>> Create (IEnumerable<VisitDto> models, CancellationToken ct)
        {
            var newVisits = _repository.Create(models);
            await _unitOfWork.SaveChangesAsync(ct);

            return newVisits.Select(item => Get(item.Id, ct).Result).ToList();
        }
        /// <summary>
        /// Deletes visit by id
        /// </summary>
        /// <param name="id">Visit's id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Boolean value</returns>
		public async Task<bool> Delete (long id, CancellationToken ct)
		{
            await _repository.DeleteHardAsync(id, ct: ct);
            await _unitOfWork.SaveChangesAsync(ct);
            var isExist = await _repository.ExistsAsync (id, ct: ct);
            return !isExist;
		}

        /// <summary>
        /// Gets visits and recogntion events bound to particular visit checking plate's numbers similarity
        /// </summary>
        /// <param name="events">Collection of recognition events</param>
        /// <returns>Collection of tuples wich consist of visits and event's collections</returns>
		public IEnumerable<Tuple<Visit, List<RecognitionEvent>>> GetVisitsFromEvents (IReadOnlyList<RecognitionEvent> events)
		{
            var visit = new Visit ();
			var visitEvents = new List<RecognitionEvent> ();
			RecognitionEvent startEvent = null;

			var list = new List<Tuple<Visit, List<RecognitionEvent>>> ();

			var eventAndVisits = new Tuple<Visit, List<RecognitionEvent>> (visit, visitEvents);

			for (var i = 0; i < events.Count; i++)
			{
				var currentEvent = events[i];
				var nextEvent = i < events.Count - 1 ? events[i + 1] : null;
				visitEvents.Add (currentEvent);
				if (startEvent == null)
				{
					startEvent = currentEvent;
					visit.Start = startEvent.Timestamp;
					visit.PlateNumber = startEvent.PlateNumber;
				}

                if (nextEvent == null) continue;
                var similarity = startEvent.PlateNumber.CalculateSimilarity (nextEvent.PlateNumber);

                if (similarity < _visitsGenerationOptions.UpperSimilarity)
                {
                    var len = nextEvent.PlateNumber.Length > 6 ? 6 : nextEvent.PlateNumber.Length;
                    var plateWithoutRegion = nextEvent.PlateNumber.Substring (0, len);

                    similarity = startEvent.PlateNumber.CalculateSimilarity (plateWithoutRegion);

                    if (similarity <= _visitsGenerationOptions.LowerSimilarity)
                    {
                        visit.End = currentEvent.Timestamp;

                        visit.Trustworthy = !((visit.End - visit.Start) < _visitsGenerationOptions.MinimalDuration);

                        list.Add (eventAndVisits);
                        startEvent = null;
                        visit = new Visit ();
                        visitEvents = new List<RecognitionEvent> ();
                        eventAndVisits = new Tuple<Visit, List<RecognitionEvent>> (visit, visitEvents);
                    }
                }
            }

			foreach (var visitAndEventsTuple in list)
			{
                var eventsByPlate = visitAndEventsTuple.Item2.GroupBy (x => x.PlateNumber).Select (x => new { num = x.Key, img = x.First ().ImageLink, cnt = x.Count () }).ToList ();
				var mostCommonPlate = eventsByPlate
					.OrderByDescending (g => g.cnt)
					.First ();

				var countOfOtherNumbers = eventsByPlate.Where (x => x.num != mostCommonPlate.num).Sum (x => x.cnt);

				double totalNumbers = mostCommonPlate.cnt + countOfOtherNumbers;
				visitAndEventsTuple.Item1.PercentAssurance = countOfOtherNumbers > 0 ? mostCommonPlate.cnt / totalNumbers : 1;
				visitAndEventsTuple.Item1.PlateNumber = mostCommonPlate.num;
            }

			return list;
        }
        /// <summary>
        /// Joins adjecent visits
        /// </summary>
        /// <param name="visits">Collection of tuples wich consist of visits and event's collections</param>
        /// <returns>Collection of tuples wich consist of visits and event's collections</returns>
        public IEnumerable<Tuple<Visit, List<RecognitionEvent>>> JoinAdjecentVisits(List<Tuple<Visit, List<RecognitionEvent>>> visits)
        {
            for (var counter = 0; counter < visits.Count - 1; counter++)
            {
                // check if numbers are equal
                if (visits[counter].Item1.PlateNumber != visits[counter + 1].Item1.PlateNumber) continue;

                // set correct ending
                visits[counter].Item1.End = visits[counter + 1].Item1.End;

                // concat recognition events
                var newEvents = visits[counter].Item2.Concat(visits[counter + 1].Item2).ToList();

                // set correct list element
                visits[counter] = new Tuple<Visit, List<RecognitionEvent>>(
                    visits[counter].Item1, newEvents);

                // remove concatenated element from the list
                visits.Remove(visits[counter + 1]);
            }

            return visits;
        }

        /// <summary>
        /// Gets visit by id
        /// </summary>
        /// <param name="id">Visit's id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Visit's object</returns>
		public async Task<VisitVm> Get (long id, CancellationToken ct)
		{
			var dto = await _repository.GetProjectionByIdAsync<VisitDto> (id, ct : ct);
			ValidateModelExists (dto);
			var result = _mapper.Map<VisitVm> (dto);
			return result;
		}

        /// <summary>
        /// Gets all visits
        /// </summary>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection of visits</returns>
		public async Task<List<VisitVm>> GetAll (CancellationToken ct)
		{
			var dtos = await _repository.ProjectToAsync<VisitDto> (x => true, ct : ct);
			var result = _mapper.Map<List<VisitVm>> (dtos.OrderByDescending (x => x.Start));
			return result;
		}

        /// <summary>
        /// Updates visit by id
        /// </summary>
        /// <param name="id">Visit's id</param>
        /// <param name="model">The object of VisitDto
        /// <see cref="VisitDto"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Updated visit</returns>
		public async Task<VisitVm> Update (long id, VisitDto model, CancellationToken ct)
		{
            await _repository.UpdateAsync(id, new Visit
            {
                Start = model.Start,
                End = model.End,
                EventsCount = model.EventsCount,
                PlateNumber = model.PlateNumber,
                IsIncorrectVisit = model.IsIncorrectVisit,
                GasStationId = model.GasStationId,
                PercentAssurance = model.PercentAssurance,
                IsClean = model.IsClean,
                Trustworthy = model.Trustworthy
            }, ct: ct);

            await _unitOfWork.SaveChangesAsync(ct);
            return await Get(id, ct);
		}

        /// <summary>
        /// Gets paged collection of visits by predicate. Overriden.
        /// </summary>
        /// <typeparam name="TProjection"></typeparam>
        /// <param name="predicate">Linq expression</param>
        /// <param name="pagination">The object of PaginationModel to enumerate and sort pages
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection of visits</returns>
        protected override async Task<Paged<TProjection>> GetPagedByPredicate<TProjection>(Expression<Func<Visit, bool>> predicate, PaginationModel pagination, CancellationToken ct)
        {
            var count = await _repository.CountAsync (predicate, ct : ct);
            var dtos = await _repository.ProjectToAsync<TProjection>(predicate, args: new PaginationArgs
                {
                    PageSize = pagination.PageSize, PageNumber = pagination.PageNumber, Sortings = pagination.Sortings ??
                        new List<SortModel>
                        {
                            new SortModel
                            {
                                FieldName = "Start",
                                Direction = SortDirections.Desc
                            }
                        }
                },
                FetchModes.NoTracking, ct);
            return new Paged<TProjection>(dtos, count, pagination.PageNumber, pagination.PageSize);
        }

        /// <summary>
        /// Gets paged collection of visits by gas station's id. Can be filtered by dates.
        /// </summary>
        /// <param name="gasStationId">Gas station's id</param>
        /// <param name="startDate">Optional. The date of visit start</param>
        /// <param name="endDate">Optional. The date of visit end</param>
        /// <param name="pagination">The object of PaginationModel to enumerate and sort pages
        /// <see cref="PaginationModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Paged collection of visits</returns>
        public async Task<Paged<VisitVm>> GetVisitsByGasStationId(long gasStationId, DateTime? startDate, DateTime? endDate, PaginationModel pagination, CancellationToken ct)
        {
            Expression<Func<Visit, bool>> predicate = x => (x.GasStationId == gasStationId)
                                                           && x.Start > (startDate ?? DateTime.MinValue) && x.End < (endDate ?? DateTime.UtcNow)
                                                           && x.Trustworthy;

            var paged = await GetPagedByPredicate<VisitDto>(predicate, pagination, ct);
            foreach (var dto in paged.Data)
            {
                dto.FullImageLink = (await _recognitionEventRepository.FindFirstAsync(x => x.VisitId == dto.Id))?.ImageLink;
            }
            var vms = _mapper.Map<List<VisitVm>>(paged.Data);
            var result = new Paged<VisitVm>(vms, paged.Total, paged.PageNumber, paged.PageSize);
            return result;
        }
	}
}
