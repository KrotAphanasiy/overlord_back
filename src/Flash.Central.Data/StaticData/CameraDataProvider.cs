using DigitalSkynet.DotnetCore.DataAccess.StaticData;
using Flash.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Flash.Central.Data.StaticData
{
    /// <summary>
    /// Class. Provides data for to camera's entity.
    /// Derived from StaticDataProvider.
    /// </summary>
    public class CameraDataProvider : StaticDataProvider<CameraDataProvider, Camera>
    {
        public static readonly Guid CAMERA_UID = new Guid("132f36ee-4f53-4043-a642-233fba6ee8c4");
        /// <summary>
        /// Generates camera's entity datas for CentralDbContext
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Camera> GenerateEntities()
        {
            yield return new Camera
            {
                Id = CAMERA_UID,
                GasStationId = 1,
                ApiKey = "Cam1Test",
                Name = "Cam1",
                NetworkAddress = "127.0.0.1",
                Port = 80,
                Login = "admin",
                Password = "admin",
                Notes = "Test",
            };
        }
    }
}
