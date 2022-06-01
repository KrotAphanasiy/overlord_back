using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Flash.Central.AdminApi.Healthchecks
{
    /// <summary>
    /// Class. Represents the liveness check
    /// </summary>
    public class LivenessCheck : IHealthCheck
    {
        /// <summary>
        /// Checks if the app is alive
        /// </summary>
        /// <param name="context">HealthCheckContext</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>The result of health's check</returns>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HealthCheckResult.Healthy("Alive"));
        }
    }
}
