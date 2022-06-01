using System.Threading;
using System.Threading.Tasks;
using Flash.Central.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Flash.Central.Api.Healthchecks
{
    
    /// <summary>
    /// Class. Checks readiness (database)
    /// </summary>
    public class ReadinessCheck : IHealthCheck
    {
        private readonly CentralDbContext _dbContext;

        /// <summary>
        /// Constructor. Initializes the class.
        /// </summary>
        /// <param name="dbContext">Represents DbContext
        /// <see cref="CentralDbContext"/>
        /// </param>
        public ReadinessCheck(CentralDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Checks if the app has the access to the database
        /// </summary>
        /// <param name="context">HealthCheckContext</param>
        /// /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>The result of health's check</returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
           _ = await _dbContext.Database.ExecuteSqlInterpolatedAsync($"select 1;", cancellationToken);
           return HealthCheckResult.Healthy("Database is up");
        }
     }
}
