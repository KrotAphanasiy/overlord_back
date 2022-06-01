using DigitalSkynet.DotnetCore.DataAccess.UnitOfWork;
using Flash.Central.Data.Repositories;
using Flash.Central.Data.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Flash.Central.Data.Configuration
{
    /// <summary>
    /// Class. Extends IServiceCollection
    /// </summary>
    public static class DataLayerDependencyConfigurator
    {
        /// <summary>
        /// Extends IServiceCollection to specify contracts for repositories classes. 
        /// </summary>
        /// <param name="services"></param>
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICameraRepository, CameraRepository>();
            services.AddScoped<ICameraRegionRepository, CameraRegionRepository>();
            services.AddScoped<IRecognitionEventRepository, RecognitionEventRepository>();
            services.AddScoped<IVisitRepository, VisitRepository>();
            services.AddScoped<IGasStationRepository, GasStationRepository>();
            services.AddScoped<ITerminalRepository, TerminalRepository>();
            services.AddScoped<IDetectionEventRepository, DetectionEventRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork<CentralDbContext>>();
        }
    }
}
