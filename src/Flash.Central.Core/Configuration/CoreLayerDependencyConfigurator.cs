using Flash.Central.Core.Services;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.Core.Validation;
using Flash.Central.Core.Validation.Interface;
using Flash.Central.Dtos.MqDtos.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Flash.Central.Core.Configuration
{
    /// <summary>
    /// Class. Extends IServiceCollection
    /// </summary>
    public static class CoreLayerDependencyConfigurator
    {
        /// <summary>
        /// Extends IServiceCollection to specify contracts for service classes.
        /// </summary>
        /// <param name="services">Extension. Specifies the contract for a collection of service descriptors.</param>
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<ICameraService, CameraService>();
            services.AddScoped<ICameraRegionService, CameraRegionService>();
            services.AddScoped<IRecognitionEventService, RecognitionEventService>();
            services.AddScoped<IPictureService, S3PictureService>();
            services.AddScoped<IVisitService, VisitService>();
            services.AddScoped<IGasStationService, GasStationService>();
            services.AddScoped<ITerminalService, TerminalService>();

            services.AddScoped<IEncodingService, EncodingService>();

            services.AddScoped<ICameraRegionValidator, CameraRegionValidator>();
            services.AddScoped<ICameraValidator, CameraValidator>();
            services.AddScoped<IGasStationValidator, GasStationValidator>();
            services.AddScoped<IRecognitionEventValidator, RecognitionEventValidator>();
            services.AddScoped<ITerminalValidator, TerminalValidator>();
            services.AddScoped<IDetectionEventService, DetectionEventService>();

            services.AddScoped<IQueueService<DetectionEventMessage>, DetectionEventQueueService>();

            services.AddSingleton<IConfigurationService, ConfigurationService>();
        }
    }
}
