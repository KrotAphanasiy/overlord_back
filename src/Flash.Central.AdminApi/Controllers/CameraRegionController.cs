using System;
using System.Threading;
using System.Threading.Tasks;
using DigitalSkynet.DotnetCore.Api.Controllers;
using DigitalSkynet.DotnetCore.DataStructures.Models.Response;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.ViewModel.Camera;
using Microsoft.AspNetCore.Mvc;

namespace Flash.Central.AdminApi.Controllers
{
    /// <summary>
    /// Class of the controller. Represents endpoints responsible for work with camera's region datas.
    /// Derived from BaseApiController.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CameraRegionController : BaseApiController
    {
        private readonly ICameraRegionService _cameraRegionService;

        /// <summary>
        /// Constructor. Initializes controller's parameters.
        /// </summary>
        /// <param name="cameraRegionService">Defines methods bound to camera's region</param>
        public CameraRegionController(ICameraRegionService cameraRegionService)
        {
            _cameraRegionService = cameraRegionService;
        }


        /// <summary>
        /// Creates a camera region
        /// </summary>
        /// <param name="model">The object of CameraRegionModel
        /// <see cref="CameraRegionModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Newly created camera's region object</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponseEnvelope<CameraRegionVm>>> CreateRegion(CameraRegionModel model,
            CancellationToken ct)
        {
            var result = await _cameraRegionService.Create(model, ct);
            return ResponseModel(result);
        }

        /// <summary>
        /// Updates a camera region by id
        /// </summary>
        /// <param name="id">camera's region id</param>
        /// <param name="model">The object of CameraRegionModel
        /// <see cref="CameraRegionModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Updated camera's region object</returns>
        [HttpPut("{id:long}")]
        public async Task<ActionResult<ApiResponseEnvelope<CameraRegionVm>>> UpdateRegion([FromRoute] long id, CameraRegionModel model,
            CancellationToken ct)
        {
            var result = await _cameraRegionService.Update(id, model, ct);
            return ResponseModel(result);
        }


        /// <summary>
        /// Deletes camera region by id
        /// </summary>
        /// <param name="id">camera's region id</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Boolean value of deletion's result</returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpDelete("{id:long}")]
        public async Task<ActionResult<ApiResponseEnvelope<bool>>> DeleteRegion([FromRoute] long id, CancellationToken ct)
        {
            var result = await _cameraRegionService.Delete(id, ct);
            return ResponseModel(result);
        }
    }
}
