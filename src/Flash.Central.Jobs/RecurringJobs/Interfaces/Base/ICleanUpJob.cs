using System.Threading.Tasks;
using Hangfire.Server;

namespace Flash.Central.Jobs.RecurringJobs.Interfaces.Base
{
    /// <summary>
    /// Interface. Specifies the contract for cleanup job classes.
    /// </summary>
    public interface ICleanUpJob
    {
        /// <summary>
        /// Cleans up
        /// </summary>
        /// <param name="context">Defines information of job context</param>
        /// <returns></returns>
        Task CleanUp(PerformContext context);
    }
}
