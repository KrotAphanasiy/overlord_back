using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flash.Domain.Entities;
using Hangfire.Server;

namespace Flash.Central.Jobs.RecurringJobs.Interfaces
{
    /// <summary>
    /// Interface. Specifies the contract for classes cleaning up visit generation.
    /// </summary>
    public interface IVisitsGenerationJob
    {
        /// <summary>
        /// Generates visit from events.
        /// </summary>
        /// <param name="context">Defines information of job context</param>
        /// <returns></returns>
        Task GenerateVisitsFromEvents(PerformContext context);
    }
}
