using Flash.Central.Core.Services.Interfaces;
using Flash.Central.Foundation.Constants;
using Flash.Central.Foundation.Enums;
using Microsoft.Extensions.Configuration;

namespace Flash.Central.Core.Services
{
    /// <summary>
    /// Class. Implements contract for all members of IConfigurationService.
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
       
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor. Initializes parameters
        /// </summary>
        /// <param name="configuration">Defines application configuration</param>
        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Returns JWT secret
        /// </summary>
        public string JwtSecret => Get<string>(Constants.JwtSectionKey, "Secret");

        /// <summary>
        /// Returns JWT audience
        /// </summary>
        public string JwtAudience => Get<string>(Constants.JwtSectionKey, "Audience");
        /// <summary>
        /// Returns JWt issuer
        /// </summary>
        public string JwtIssuer => Get<string>(Constants.JwtSectionKey, "Issuer");
        /// <summary>
        /// Returns JWT expiration
        /// </summary>
        public string JwtExpireSeconds => Get<string>(Constants.JwtSectionKey, "ExpireSeconds");

        /// <summary>
        /// Gets JWT camera token secret
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        public string JwtCameraTokenSecret => Get<string>(Constants.JwtSectionKey, "CameraTokenSecret");

        /// <summary>
        /// Gets JWT camera token salt
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        public string JwtCameraTokenSalt => Get<string>(Constants.JwtSectionKey, "CameraTokenSalt");

        /// <summary>
        /// Gets connection string to connect to database
        /// </summary>
        /// <returns></returns>
        public string ConnectionString => GetConnectionString("DefaultConnection");

        /// <summary>
        /// Gets migration update mode
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        public MigrationMode MigrationsMode => Get<MigrationMode>(string.Empty, "Migrate");

        /// <summary>
        /// Gets parameter indicating is automigrations enbabled
        /// </summary>
        /// <typeparam name="bool"></typeparam>
        /// <returns></returns>
        public bool MigrationAllowAutoMigrations => Get<bool>("Migration", "AllowAutoMigrations");

        /// <summary>
        /// Gets hangfire is enabled
        /// </summary>
        /// <typeparam name="bool"></typeparam>
        /// <returns></returns>
        public bool IsHangifreEnabled => Get<bool>("Hangfire", "Enabled");

        /// <summary>
        /// Gets hangfire storage mode
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        public HangfireStorageMode HangFireStorageMode => Get<HangfireStorageMode>("Hangfire", "Storage");

        /// <summary>
        /// Gets hangfire connection string
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        public string HangFireConnectionString => Get<string>("Hangfire", "ConnectionString");

        /// <summary>
        /// Gets hangfire default cron expression
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        public string HangFireCronExpression => Get<string>("Hangfire", "CronExpression");

        /// <summary>
        /// Gets app settings
        /// </summary>
        /// <param name="section">JSON section</param>
        /// <param name="key">Params key</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Object</returns>
        private T Get<T>(string section, string key)
        {
            T result = default(T);
            if (string.IsNullOrEmpty(section))
            {
                result = _configuration.GetValue<T>(key);
            }
            else
            {
                result = _configuration.GetSection(section).GetValue<T>(key);
            }

            return result;
        }
        /// <summary>
        /// Gets connection string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetConnectionString(string key)
        {
            return _configuration.GetConnectionString(key);
        }
    }
}
