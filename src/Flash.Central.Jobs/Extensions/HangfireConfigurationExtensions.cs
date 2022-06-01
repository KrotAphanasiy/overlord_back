using System;
using Flash.Central.Foundation.Enums;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.Foundation.Options;
using Flash.Central.Jobs.RecurringJobs.Interfaces;
using Hangfire;
using Hangfire.Console;
using Hangfire.Dashboard;
using Hangfire.MemoryStorage;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Flash.Central.Jobs.Extensions
{
    /// <summary>
    /// Class. Extends IServiceCollection
    /// </summary>
    public static class HangfireConfigurationExtensions
    {
        /// <summary>
        /// Constant. Defines the maximum number of running background job if fails
        /// </summary>
        public const int MaxJobAttemptsCount = 3;

        /// <summary>
        /// Configures hangfire
        /// </summary>
        /// <param name="services">Extension. Specifies the contract for a collection of service descriptors.</param>
        public static void SetupHangfireConfiguration(this IServiceCollection services)
        {

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            IConfigurationService config = serviceProvider.GetRequiredService<IConfigurationService>();

            if (config.IsHangifreEnabled)
            {
                services.AddHangfire(hangfireConfig =>
                {
                    hangfireConfig
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseConsole();
                    switch (config.HangFireStorageMode)
                    {
                        case HangfireStorageMode.Memory:
                            hangfireConfig.UseMemoryStorage();
                            break;
                        case HangfireStorageMode.Postgres:
                            hangfireConfig.UsePostgreSqlStorage(config.ConnectionString);
                            break;
                    }
                });

                GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
                {
                    Attempts = MaxJobAttemptsCount,
                        OnAttemptsExceeded = AttemptsExceededAction.Fail
                });
                services.AddHangfireServer();
            }
        }

        /// <summary>
        /// Uses hangfire
        /// </summary>
        /// <param name="app"></param>
        public static void UseHangfireConfiguration(this IApplicationBuilder app)
        {
            var config = app.ApplicationServices.GetRequiredService<IConfigurationService>();

            if (!config.IsHangifreEnabled) return;

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = Array.Empty<IDashboardAuthorizationFilter>()
            });

            var visitsCronExpression = config.HangFireCronExpression;

            RecurringJob.AddOrUpdate<IVisitsGenerationJob>
            ((generateVisitsJob) => generateVisitsJob
                    .GenerateVisitsFromEvents(null),
                    visitsCronExpression
            );

            var picturesCleanUpOptions = app.ApplicationServices.GetRequiredService<IOptions<PicturesCleanupOptions>>();

            RecurringJob.AddOrUpdate<IPicturesCleanUpJob>
            ((picturesCleanupJob) => picturesCleanupJob
                    .CleanUp(null),
                    picturesCleanUpOptions.Value.CleanupCronExpression
            );

            var detectionEventsCleanUpOptions = app.ApplicationServices.GetRequiredService<IOptions<DetectionEventsCleanUpOptions>>().Value;

            RecurringJob.AddOrUpdate<IDetectionEventsCleanUpJob>
            ((detectionEventsCleanUpJob) => detectionEventsCleanUpJob
                    .CleanUp(null),
                    detectionEventsCleanUpOptions.CronExpression
            );

            var recognitionEventsCleanUpOptions = app.ApplicationServices.GetRequiredService<IOptions<RecognitionEventsCleanUpOptions>>().Value;

            RecurringJob.AddOrUpdate<IRecognitionEventsCleanUpJob>
            ((recognitionEventsCleanUpJob) => recognitionEventsCleanUpJob
                    .CleanUp(null),
                    recognitionEventsCleanUpOptions.CronExpression
            );
        }

    }
}
