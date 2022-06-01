using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigitalSkynet.DotnetCore.DataAccess.UnitOfWork;
using Flash.Central.Data.Repositories.Interfaces;
using Flash.Central.Dtos.RecognitionEvent;
using Flash.Central.Foundation.Options;
using Flash.Central.Jobs.RecurringJobs.Interfaces;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Extensions.Options;

namespace Flash.Central.Jobs.RecurringJobs
{
    /// <summary>
    /// Class. Implements contract with IRecognitionEventsCleanUpJob
    /// </summary>
    public class RecognitionEventsCleanUpJob : IRecognitionEventsCleanUpJob
    {
        private readonly RecognitionEventsCleanUpOptions _cleanUpOptions;
        private readonly IRecognitionEventRepository _recognitionEventRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor. Initializes parameters
        /// </summary>
        /// <param name="cleanUpOptions">Defines RecognitionEventsCleanUpOptions</param>
        /// <param name="recognitionEventRepository">Defines IRecognitionEventRepository</param>
        /// <param name="unitOfWork">Defines IUnitOfWork methods</param>
        public RecognitionEventsCleanUpJob(
            IOptions<RecognitionEventsCleanUpOptions> cleanUpOptions,
            IRecognitionEventRepository recognitionEventRepository,
            IUnitOfWork unitOfWork
        )
        {
            _cleanUpOptions = cleanUpOptions.Value;
            _recognitionEventRepository = recognitionEventRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Deletes recognition events according the certain time
        /// </summary>
        /// <param name="context">Provides information about context</param>
        /// <returns>Completed task</returns>
        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        public async Task CleanUp(PerformContext context)
        {
            var ct = context?.CancellationToken.ShutdownToken ?? CancellationToken.None;

            var eventsToDelete = await _recognitionEventRepository.ProjectToAsync<RecognitionEventDto>(
                x => x.VisitId != null
                     && ((x.Timestamp + _cleanUpOptions.EventLifetime) < DateTime.UtcNow), ct: ct
            );

            foreach (var eventDto in eventsToDelete)
            {
                await _recognitionEventRepository.DeleteHardAsync(eventDto.Id, ct: ct);
            }

            context?.WriteLine("Deleted {0} recognition events", eventsToDelete.Count);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
