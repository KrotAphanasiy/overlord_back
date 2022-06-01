using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using DigitalSkynet.DotnetCore.DataAccess.Enums;
using DigitalSkynet.DotnetCore.DataAccess.UnitOfWork;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.Data.Repositories.Interfaces;
using Flash.Central.Dtos.RecognitionEvent;
using Flash.Central.Foundation.Options;
using Flash.Central.Jobs.RecurringJobs.Interfaces;
using Flash.Central.ViewModel.Visit;
using Flash.Domain.Entities;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Extensions.Options;

namespace Flash.Central.Jobs.RecurringJobs
{
    /// <summary>
    /// Class. Implements contract with IVisitsGenerationJob
    /// </summary>
    public class VisitsGenerationJob : IVisitsGenerationJob
    {
        private readonly IVisitService _visitService;
        private readonly IVisitRepository _visitRepository;
        private readonly IRecognitionEventRepository _recognitionEventRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly VisitsGenerationOptions _options;
        /// <summary>
        /// Constructor. Initializes parameters
        /// </summary>
        /// <param name="visitService">Defines IVisitService</param>
        /// <param name="options">Defines VisitsGenerationOptions</param>
        /// <param name="recognitionEventRepository">Defines IRecognitionEventRepository</param>
        /// <param name="visitRepository">Defines IVisitRepository</param>
        /// <param name="unitOfWork">Defines IUnitOfWork methods</param>
        public VisitsGenerationJob(
            IVisitService visitService,
            IOptions<VisitsGenerationOptions> options,
            IRecognitionEventRepository recognitionEventRepository,
            IVisitRepository visitRepository,
            IUnitOfWork unitOfWork)
        {
            _visitService = visitService;
            _recognitionEventRepository = recognitionEventRepository;
            _visitRepository = visitRepository;
            _unitOfWork = unitOfWork;
            _options = options.Value;
        }
        /// <summary>
        /// Generates visits according to the recognition events by checking plate numbers
        /// </summary>
        /// <param name="context">Provides information about context</param>
        /// <returns>Completed task</returns>
        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        public async Task GenerateVisitsFromEvents(PerformContext context)
        {

           var ct = context?.CancellationToken.ShutdownToken ?? CancellationToken.None;

            context?.WriteLine("Progress on visits:");
            var progressOnVisits = context?.WriteProgressBar();
            context?.WriteLine("Progress on events per visit:");
            var progressOnEvents = context?.WriteProgressBar();

            //TODO: fix time range of events without visits
			var eventsWithoutVisits = await _recognitionEventRepository.FilterAsync (x =>
                x.VisitId == null
                && x.Timestamp > _options.StartingDate && !x.IsIncorrectNumber, FetchModes.Tracking, ct : ct);

			eventsWithoutVisits = eventsWithoutVisits.OrderBy(x => x.CameraRegionId).ThenBy(x => x.Timestamp).ToList();


            //TODO: this is a;ready done in recognizer, so we don`t need regex check there anymore, but it does no harm
			const string characters = "ABEKMHOPCTYX";

			var plateNumberRegex = $"^[{characters}][0-9]{{3}}[{characters}]{{2}}[0-9]{{1}}[1-9]{{1,2}}$";

			var goodNumbers = new List<RecognitionEvent> ();

            foreach (var x in eventsWithoutVisits)
			{
				if (Regex.IsMatch(x.PlateNumber, plateNumberRegex) &&
					int.Parse(x.PlateNumber.Substring (1, 3)) > 0 &&
                    int.Parse (x.PlateNumber.Substring (6)) > 0)
				{
					goodNumbers.Add (x);
				}
            }

            var visitsFromEvents = _visitService.GetVisitsFromEvents(goodNumbers).ToList();

            // extract trustworthy visits and join adjecent ones, ignoring untrustworthy
			var enumerableVisits = _visitService.JoinAdjecentVisits(visitsFromEvents
                .Where(x => x.Item1.Trustworthy).ToList()).ToList();

            // concat both trustworthy and untrustworthy visits, cause we need to save them all
            enumerableVisits = enumerableVisits.Concat(visitsFromEvents.Where(x => !x.Item1.Trustworthy)).ToList();

            foreach (var (visit, events) in enumerableVisits.WithProgress(progressOnVisits))
            {
                if(!events.Any()) continue;
                var eventId = events.First().Id;
                Expression<Func<RecognitionEvent, bool>> predicate = x => (x.Id == eventId);

                var eventDto = await _recognitionEventRepository.ProjectToAsync<RecognitionEventDto>(predicate, ct: ct);

                visit.EventsCount = events.Count;

                visit.GasStationId = eventDto.Select(x => x.GasStationId).FirstOrDefault();

                var visitModel = new VisitModel
                {
                    Start = visit.Start,
                    End = visit.End,
                    EventsCount = visit.EventsCount,
                    IsIncorrectVisit = visit.IsIncorrectVisit,
                    PlateNumber = visit.PlateNumber,
                    GasStationId = visit.GasStationId,
                    PercentAssurance = visit.PercentAssurance,
                    Trustworthy = visit.Trustworthy
                };

                var newVisit = _visitRepository.Create(visitModel);
                await _unitOfWork.SaveChangesAsync(ct);

                foreach (var eventWithoutVisit in events.WithProgress(progressOnEvents))
                {
                    eventWithoutVisit.Visit = newVisit;
                    eventWithoutVisit.VisitId = newVisit.Id;
                    await _recognitionEventRepository.UpdateAsync(eventWithoutVisit.Id, eventWithoutVisit, ct: ct);
                }
                await _unitOfWork.SaveChangesAsync(ct);
            }
        }
    }
}
