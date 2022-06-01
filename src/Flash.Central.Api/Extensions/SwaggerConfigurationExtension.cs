using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;

namespace Flash.Central.Api.Extensions
{
    /// <summary>
    /// Class. Represents Swagger setup middleware
    /// </summary>
    public static class SwaggerConfigurationExtension
    {
        /// <summary>
        /// Adds swagger documentation
        /// </summary>
        /// <param name="services">Extension of IServiceCollection. Specifies the contract for a collection of service descriptors</param>
        public static void AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "FLASH Central API",
                    Description = "Flash"
                });

                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme()
                {
                    Description = "Authorization header using the Camera Api Key scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        new List<string>()
                    }
                });

                c.DescribeAllParametersInCamelCase();
                c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath,
                    $"{PlatformServices.Default.Application.ApplicationName}.xml"));
            });
        }

        /// <summary>
        /// Configures swagger documentation
        /// </summary>
        /// <param name="app">Extension of IApplicationBuilder. Defines the mechanisms to configure an application's request pipeline</param>
        public static void UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger(s =>
            {
                s.RouteTemplate = "api/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/v1/swagger.json", "Flash API V1");
                c.RoutePrefix = "swagger";
            });
        }
    }
}
