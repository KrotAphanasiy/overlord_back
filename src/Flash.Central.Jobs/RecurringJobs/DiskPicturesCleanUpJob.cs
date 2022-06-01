using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigitalSkynet.DotnetCore.DataAccess.UnitOfWork;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.Data.Repositories.Interfaces;
using Flash.Central.Dtos.RecognitionEvent;
using Flash.Central.Dtos.Visit;
using Flash.Central.Foundation.Options;
using Flash.Central.Jobs.RecurringJobs.Interfaces;
using Flash.Domain.Entities;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Extensions.Options;


namespace Flash.Central.Jobs.RecurringJobs
{
    /// <summary>
    /// Class. Implements contract with IPicturesCleanUpJob
    /// </summary>
    public class DiskPicturesCleanUpJob : IPicturesCleanUpJob
    {
        private readonly IPictureService _pictureService;
        private readonly IRecognitionEventService _recognitionEventService;
        private readonly IVisitService _visitService;
        private readonly PicturesCleanupOptions _options;

        private readonly IRecognitionEventRepository _recognitionEventRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor. Initializes parameters
        /// </summary>
        /// <param name="pictureService">Defines IPictureService</param>
        /// <param name="detectionEventService">Defines IDetectionEventService</param>
        /// <param name="recognitionEventService">Defines IRecognitionEventService</param>
        /// <param name="visitService">Defines IVisitService</param>
        /// <param name="options">Defines PicturesCleanupOptions</param>
        /// <param name="recognitionEventRepository">Defines IRecognitionEventRepository</param>
        /// <param name="unitOfWork">Defines IUnitOfWork methods</param>
        public DiskPicturesCleanUpJob(IPictureService pictureService,
            IDetectionEventService detectionEventService,
            IRecognitionEventService recognitionEventService,
            IVisitService visitService,
            IOptions<PicturesCleanupOptions> options,

            IRecognitionEventRepository recognitionEventRepository,
            IUnitOfWork unitOfWork)
        {
            _pictureService = pictureService;
            _recognitionEventService = recognitionEventService;
            _visitService = visitService;
            _options = options.Value;
            _recognitionEventRepository = recognitionEventRepository;
            _unitOfWork = unitOfWork;
        }


        //TODO: replace as much manual model constructs as possible with mapping
        //TODO: encapsulate some actions to other job`s private methods
        /// <summary>
        /// Deletes pictures according the certain time and updates detetection events, recognition events and visits
        /// </summary>
        /// <param name="context">Provides information about context</param>
        /// <returns>Completed task</returns>
        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        public async Task CleanUp(PerformContext context)
        {
            var ct = CancellationToken.None;

            if (context != null) ct = context.CancellationToken.ShutdownToken;

            context?.WriteLine("Progress on full pictures");
            var fullPicturesProgress = context?.WriteProgressBar();
            context?.WriteLine("Progress on cropped pictures");
            var croppedPicturesProgress = context?.WriteProgressBar();

            var fullPicturesLifetime = _options.FullPicturesLifetime;
            var croppedPicturesLifetime = _options.CroppedPicturesLifetime;

            var fullRecoPictures = await _recognitionEventService.GetImageLinksFromProcessedEvents(
                _options.PicturesToKeep, DateTime.UtcNow - fullPicturesLifetime, ct);
            var croppedRecoPictures = await _recognitionEventService.GetProcessedImageLinksFromProcessedEvents(
                _options.PicturesToKeep, DateTime.UtcNow - croppedPicturesLifetime, ct);


            foreach (var (link, visitId, _) in fullRecoPictures.WithProgress(fullPicturesProgress))
            {
                await _pictureService.Delete(link, ct);
            }

            foreach (var (link, visitId, _) in croppedRecoPictures.WithProgress(croppedPicturesProgress))
            {
                await _pictureService.Delete(link, ct);
                var visitToUpdate = await _visitService.Get(visitId, ct);
                var updatedVisit = await _visitService.Update(visitId, new VisitDto
                {
                    Start = visitToUpdate.Start,
                    End = visitToUpdate.End,
                    EventsCount = visitToUpdate.EventsCount,
                    PlateNumber = visitToUpdate.PlateNumber,
                    IsIncorrectVisit = visitToUpdate.IsIncorrectVisit,
                    GasStationId = visitToUpdate.GasStationId,
                    GasStationName = visitToUpdate.GasStationName,
                    IsClean = true
                }, ct);

            }

            await _unitOfWork.SaveChangesAsync(ct);

            // replacing image links in full pics
            var distinctVisitsAndLinks = (from fullRecoPicture in fullRecoPictures
                select (fullRecoPicture.Item2, fullRecoPicture.Item3)).Distinct();

            foreach (var (visitId, replacingLink) in distinctVisitsAndLinks)
            {
                var visitEvents = await _recognitionEventRepository.ProjectToAsync<RecognitionEventDto>(x => x.VisitId == visitId, ct: ct);

                foreach (var visitEvent in visitEvents)
                {
                    await _recognitionEventRepository.UpdateAsync(visitEvent.Id, new RecognitionEvent
                    {
                        PlateNumber = visitEvent.PlateNumber,
                        Probability = visitEvent.Probability,
                        Timestamp = visitEvent.Timestamp,

                        ImageLink = replacingLink,

                        ProcessedImageLink = visitEvent.ProcessedImageLink,
                        CameraRegionId = visitEvent.CameraRegionId,
                        IsIncorrectNumber = visitEvent.IsIncorrectNumber,
                        VisitId = visitEvent.VisitId
                    }, ct: ct);
                }
                await _unitOfWork.SaveChangesAsync(ct);
            }


            // replacing image links in cropped pics
            distinctVisitsAndLinks = (from croppedRecoPicture in croppedRecoPictures
                select (croppedRecoPicture.Item2, croppedRecoPicture.Item3)).Distinct();

            foreach (var (visitId, replacingLink) in distinctVisitsAndLinks)
            {
                var visitEvents = await _recognitionEventRepository.ProjectToAsync<RecognitionEventDto>(x => x.VisitId == visitId, ct: ct);

                foreach (var visitEvent in visitEvents)
                {
                    await _recognitionEventRepository.UpdateAsync(visitEvent.Id, new RecognitionEvent
                    {
                        PlateNumber = visitEvent.PlateNumber,
                        Probability = visitEvent.Probability,
                        Timestamp = visitEvent.Timestamp,
                        ImageLink = visitEvent.ImageLink,

                        ProcessedImageLink = replacingLink,

                        CameraRegionId = visitEvent.CameraRegionId,
                        IsIncorrectNumber = visitEvent.IsIncorrectNumber,
                        VisitId = visitEvent.VisitId
                    }, ct: ct);
                }
                await _unitOfWork.SaveChangesAsync(ct);
            }
        }

    }
}
