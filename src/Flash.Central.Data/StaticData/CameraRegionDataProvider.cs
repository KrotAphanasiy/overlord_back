using System;
using System.Collections.Generic;
using DigitalSkynet.DotnetCore.DataAccess.StaticData;
using Flash.Domain.Entities;

namespace Flash.Central.Data.StaticData
{
    /// <summary>
    /// Class. Provides data for to camera's region entity.
    /// Derived from StaticDataProvider.
    /// </summary>
    public class CameraRegionDataProvider : StaticDataProvider<CameraRegionDataProvider, CameraRegion>
    {
        public static readonly Guid CAMERA_REGION_UID = new Guid("fa9b4486-5355-4510-8e5e-c7d19aadc528");
        /// <summary>
        /// Generates camera's region entity datas for CentralDbContext
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<CameraRegion> GenerateEntities()
        {
            yield return new CameraRegion
            {
                Id = 1,
                TopLeftX = 0,
                TopLeftY = 0,
                BottomRightX = 1920,
                BottomRightY = 1080,
                TerminalId = null,
                CameraId = CameraDataProvider.CAMERA_UID
            };
        }
    }
}
