using Flash.Central.Foundation.Enums;

namespace Flash.Central.Core.Services.Interfaces
{
    /// <summary>
    /// Interface. Specifies the contract for configuration services.
    /// Includes database, jwt, hangfire configuration.
    /// </summary>
    public interface IConfigurationService
    {
        string ConnectionString { get; }
        string JwtSecret { get; }
        string JwtAudience { get; }
        string JwtIssuer { get; }
        string JwtExpireSeconds { get; }
        string JwtCameraTokenSecret { get; }
        string JwtCameraTokenSalt { get; }
        MigrationMode MigrationsMode { get; }
        bool MigrationAllowAutoMigrations { get; }
        bool IsHangifreEnabled { get; }
        HangfireStorageMode HangFireStorageMode { get; }
        string HangFireConnectionString { get; }
        string HangFireCronExpression { get; }
    }
}
