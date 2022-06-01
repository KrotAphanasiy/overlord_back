using DigitalSkynet.DotnetCore.DataAccess.StaticData;
using Flash.Domain.Entities;
using System.Collections.Generic;

namespace Flash.Central.Data.StaticData
{
    /// <summary>
    /// Class. Provides data for to gas station entity.
    /// Derived from StaticDataProvider.
    /// </summary>
    public class GasStationDataProvider : StaticDataProvider<GasStationDataProvider, GasStation>
    {
        /// <summary>
        /// Generates gas station's entity datas for CentralDbContext
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<GasStation> GenerateEntities()
        {
            yield return new GasStation
            {
                Id = 1,
                Name = "Test",
                GasPumpCount = 5
            };
        }
    }
}
