using System;
using Flash.Central.Data;
using Flash.Central.Foundation.Enums;
using Flash.Central.Core.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Flash.Central.Api.Extensions
{
    /// <summary>
    /// Class. Extends IApplicationBuilder 
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Implements databse migration.
        /// </summary>
        /// <param name="app">Extension of IApplicationBuilder. Defines the mechanisms to configure an application's request pipeline</param>
        /// <param name="configuration">Defines configuration settings</param>
        public static void Migrate(this IApplicationBuilder app, IConfigurationService configuration)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            using var context = serviceScope.ServiceProvider.GetRequiredService<CentralDbContext>();
            var logger = serviceScope.ServiceProvider
                .GetRequiredService<ILogger<Startup>>();

            if (configuration.MigrationsMode != MigrationMode.Up)
            {
                logger.LogInformation("Database is not going to be migrated, because MigrationMode is not Up");
                return;
            }

            if (!configuration.MigrationAllowAutoMigrations)
            {
                logger.LogInformation("Database is not going to be migrated, because auto migrations are`nt allowed");
                return;
            }

            try
            {
                context.Database.Migrate();
                logger.LogInformation("Database migrated successfully");
            }
            catch (Exception e)
            {
                logger.LogCritical("Cannot migrate database: {0} \n{1}", e.Message, e.StackTrace);
                throw;
            }

        }
    }
}
