using Flash.Central.Jobs.RecurringJobs;
using Flash.Central.Jobs.RecurringJobs.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Flash.Central.Jobs.Configuration
{
    /// <summary>
    /// Class. Extends IServiceCollection
    /// </summary>
    public static class JobsLevelDependencyConfigurator
    {
        /// <summary>
        /// Extends IServiceCollection to specify contracts for job's classes
        /// </summary>
        /// <param name="services">Extension. Specifies the contract for a collection of service descriptors.</param>
        /// <returns></returns>
        public static IServiceCollection AddJobs(this IServiceCollection services)
        {
            services.AddTransient<IPicturesCleanUpJob, DiskPicturesCleanUpJob>();
            services.AddTransient<IVisitsGenerationJob, VisitsGenerationJob>();
            services.AddTransient<IDetectionEventsCleanUpJob, DetectionEventsCleanUpJob>();
            services.AddTransient<IRecognitionEventsCleanUpJob, RecognitionEventsCleanUpJob>();
            return services;
        }
    }
}
