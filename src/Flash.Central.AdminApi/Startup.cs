using DigitalSkynet.DotnetCore.Api.Middleware;
using Flash.Central.AdminApi.Extensions;
using Flash.Central.Core.Configuration;
using Flash.Central.Core.Services;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.Core.Validation;
using Flash.Central.Data;
using Flash.Central.Data.Configuration;
using Flash.Central.Foundation.Options;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using Flash.Central.AdminApi.Healthchecks;
using Flash.Central.Foundation.Constants;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;


namespace Flash.Central.AdminApi
{
    /// <summary>
    /// Application startup config
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
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

                options.UseNpgsql(config.ConnectionString, x =>
                {
                    x.MigrationsAssembly(Constants.MigrationDataProject);
                });
            });

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

            services.AddAutoMapper(AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(x => x.FullName.Contains($"{Constants.ProjectNamespaceMain}.")));

            services.AddRepositories();
            services.ConfigureServices();

            services.Configure<ImageUploadOptions>(Configuration.GetSection("Uploads"));
            services.Configure<KafkaDetectionEventOptions>(Configuration
                .GetSection("Kafka")
                .GetSection("DetectionMessages"));

            services.Configure<VersionOptions>(Configuration.GetSection("Version"));

            services.Configure<S3ClientOptions>(Configuration
                .GetSection("S3"));
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">IWebHostEnvironment</param>
        /// <param name="autoMapper">IConfigurationProvider of AutoMapper</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AutoMapper.IConfigurationProvider autoMapper)
        {
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
            autoMapper.AssertConfigurationIsValid();

            app.UseEndpoints(endpoints => {
                endpoints.MapHealthChecks("/health/liveness", new HealthCheckOptions { Predicate = x => x.Name == "liveness" });
                endpoints.MapHealthChecks("/health/readiness", new HealthCheckOptions { Predicate = x => x.Name == "readiness" });

                endpoints.MapControllers();
            });
        }
    }
}
