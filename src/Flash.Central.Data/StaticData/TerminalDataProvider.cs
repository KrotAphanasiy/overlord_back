using DigitalSkynet.DotnetCore.DataAccess.StaticData;
using Flash.Domain.Entities;
using System.Collections.Generic;

namespace Flash.Central.Data.StaticData
{
    /// <summary>
    /// Class. Provides data for to terminal entity.
    /// Derived from StaticDataProvider.
    /// </summary>
    public class TerminalDataProvider : StaticDataProvider<TerminalDataProvider, Terminal>
    {
        /// <summary>
        /// Generates terminals entity datas for CentralDbContext
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Terminal> GenerateEntities()
        {
            yield return new Terminal
            {
                Id = 1,
                Name = "Test"
            };
        }
    }
}
