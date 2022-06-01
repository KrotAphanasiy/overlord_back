using System;
using System.Threading;
using System.Threading.Tasks;
using DigitalSkynet.DotnetCore.DataAccess.UnitOfWork;
using Flash.Central.Data.Repositories.Interfaces;
using Flash.Central.Dtos.DetectionEvent;
using Flash.Central.Foundation.Options;
using Flash.Central.Jobs.RecurringJobs.Interfaces;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Extensions.Options;

namespace Flash.Central.Jobs.RecurringJobs
{
    /// <summary>
    /// Class. Implements contract with IDetectionEventsCleanUpJob
    /// </summary>
    public class DetectionEventsCleanUpJob : IDetectionEventsCleanUpJob
    {
        private readonly DetectionEventsCleanUpOptions _cleanUpOptions;
        private readonly IDetectionEventRepository _detectionEventRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor. Initializes parameters
        /// </summary>
        /// <param name="cleanUpOptions">Defines DetectionEventsCleanUpOptions</param>
        /// <param name="detectionEventRepository">Defines IDetectionEventRepository</param>
        /// <param name="unitOfWork">Defines IUnitOfWork methods</param>
        public DetectionEventsCleanUpJob(
            IOptions<DetectionEventsCleanUpOptions> cleanUpOptions,
            IDetectionEventRepository detectionEventRepository,
            IUnitOfWork unitOfWork
        )
        {
            _cleanUpOptions = cleanUpOptions.Value;
            _detectionEventRepository = detectionEventRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Deletes detection events according the certain time
        /// </summary>
        /// <param name="context">Provides information about context</param>
        /// <returns>Completed task</returns>
        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        public async Task CleanUp(PerformContext context)
        {
            var ct = context?.CancellationToken.ShutdownToken ?? CancellationToken.None;

            var eventsToDelete = await _detectionEventRepository.ProjectToAsync<DetectionEventDto>(
                x =>
                    x.Processed
                    && ((x.Timestamp + _cleanUpOptions.EventLifetime) < DateTime.UtcNow), ct: ct);

            context?.WriteLine("Deleted {0} detection events", eventsToDelete.Count);

            foreach (var eventDto in eventsToDelete)
            {
                await _detectionEventRepository.DeleteHardAsync(eventDto.Id, ct: ct);
            }

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
