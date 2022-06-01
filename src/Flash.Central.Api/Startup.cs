using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using DigitalSkynet.DotnetCore.Api.Middleware;
using Flash.Central.Api.Extensions;
using Flash.Central.Api.Healthchecks;
using Flash.Central.Data;
using Flash.Central.Data.Configuration;
using Flash.Central.Foundation.Options;
using Flash.Central.Core.Configuration;
using Flash.Central.Core.Services;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.Core.Validation;
using Flash.Central.Foundation.Constants;
using Flash.Central.Jobs.Configuration;
using Flash.Central.Jobs.Extensions;
using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

[assembly: ApiController]
namespace Flash.Central.Api
{
    /// <summary>
    /// Class. Used to initial configuration of the application
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor. Initializes startup class.
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the system application's configuration
        /// </summary>
        /// <value></value>
        private IConfiguration Configuration
        {
            get;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CentralDbContext>((serviceProvider, options) =>
            {
                if (Debugger.IsAttached)
                {
                    options.EnableSensitiveDataLogging();
                    options.UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>());
                }

                var config = serviceProvider.GetRequiredService<IConfigurationService>();

                options.UseSqlServer(config.ConnectionString, x =>
                {
                    x.MigrationsAssembly(Constants.MigrationDataProject);
                });
            });

            services.AddDataProtection()
                .PersistKeysToDbContext<CentralDbContext>();


            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy
                    {
                        OverrideSpecifiedNames = false
                    }
                },
                Formatting = Formatting.Indented
            };

            services.AddMvc(options =>
                {
                    options.AllowEmptyInputInBodyModelBinding = true;
                    options.EnableEndpointRouting = false;

                })
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase)
                .AddFluentValidation(fv =>
                {
                    fv.LocalizationEnabled = false;
                    fv.RegisterValidatorsFromAssembly(typeof(RecognitionEventValidator).Assembly);
                })
                .AddControllersAsServices();

            services.AddHealthChecks()
                .AddCheck<LivenessCheck>("liveness", tags: new[] { "liveness" })
                .AddCheck<ReadinessCheck>("readiness", tags: new[] { "readiness" });

            services.AddSwaggerDocumentation();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddJobs();

            // TODO: configure options

            services.AddAutoMapper(AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(x => x.FullName != null && x.FullName.Contains($"{Constants.ProjectNamespaceMain}.")));

            services.AddRepositories();

            services.ConfigureServices();

            services.SetupAuthorization();

            services.SetupHangfireConfiguration();

            services.Configure<ImageUploadOptions>(Configuration
                .GetSection("Uploads"));

            services.Configure<KafkaDetectionEventOptions>(Configuration
                .GetSection("Kafka")
                .GetSection("DetectionMessages"));

            services.Configure<PicturesCleanupOptions>(Configuration
                .GetSection("Hangfire")
                .GetSection("PicturesCleanup"));

            services.Configure<DetectionEventsCleanUpOptions>(Configuration
                .GetSection("Hangfire")
                .GetSection("DetectionEventsCleanUp"));

            services.Configure<RecognitionEventsCleanUpOptions>(Configuration
                .GetSection("Hangfire")
                .GetSection("DetectionEventsCleanUp"));

            services.Configure<SuperuserOptions>(Configuration
                .GetSection("Superuser"));

            services.Configure<VisitsGenerationOptions>(Configuration
                .GetSection("VisitsGeneration"));

            services.Configure<VersionOptions>(Configuration
                .GetSection("Version"));

            services.Configure<S3ClientOptions>(Configuration
                .GetSection("S3"));

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="autoMapper"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            AutoMapper.IConfigurationProvider autoMapper)
        {
            var config = app.ApplicationServices.GetRequiredService<IConfigurationService>();

            app.Migrate(config);

            app.UseGlobalExceptionHandler(); // todo: override it and log errors to Sentry

            var corsOptions = Configuration.GetSection("Cors").Get<CorsOptions>();

            app.UseCors(builder => builder
                .WithOrigins(corsOptions.AllowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseSwaggerDocumentation();

            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/images"
            });

            app.UseRouting();
            app.UseAuthorization();

            app.UseHangfireConfiguration();

            autoMapper.AssertConfigurationIsValid();

            app.UseEndpoints(endpoints => {
                endpoints.MapHealthChecks("/health/liveness", new HealthCheckOptions { Predicate = x => x.Name == "liveness" });
                endpoints.MapHealthChecks("/health/readiness", new HealthCheckOptions { Predicate = x => x.Name == "readiness" });

                endpoints.MapControllers();
            });

        }
    }
}
