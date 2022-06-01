using DigitalSkynet.DotnetCore.Api.Controllers;
using DigitalSkynet.DotnetCore.DataStructures.Models.Response;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.ViewModel.Camera;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Flash.Central.AdminApi.Controllers
{
    /// <summary>
    /// Class of the controller. Represents endpoints responsible for work with camera's datas.
    /// Derived from BaseApiController.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CameraController : BaseApiController
    {
        private readonly ICameraService _cameraService;
        private readonly IEncodingService _encodingService;

        /// <summary>
        /// Constructor. Initializes controller's parameters.
        /// </summary>
        /// <param name="cameraService">Defines methods bound to camera</param>
        /// <param name="encodingService">Defines methods bound to encoding and decoding</param>
        public CameraController(ICameraService cameraService, IEncodingService encodingService)
        {
            _cameraService = cameraService;
            _encodingService = encodingService;
        }

        /// <summary>
        /// Gets all cameras from database
        /// </summary>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection of cameras</returns>
        [HttpGet]
        public async Task<ActionResult<ApiCollectionResponseEnvelope<CameraVm>>> GetAll(CancellationToken ct = default)
        {
            var result = await _cameraService.GetAll(ct);
            return CollectionResponse(result);
        }

        /// <summary>
        /// Gets cameras for gas station by it`s id
        /// </summary>
        /// <param name="gasStationId">The guid of gas station</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Collection of cameras</returns>
        [HttpGet("station/{gasStationId:long}")]
        public async Task<ActionResult<ApiCollectionResponseEnvelope<CameraVm>>> GetForStation([FromRoute] long gasStationId, CancellationToken ct = default)
        {
            var result = await _cameraService.GetForStation(gasStationId, ct);
            return CollectionResponse(result);
        }

        /// <summary>
        /// Gets camera by id
        /// </summary>
        /// <param name="uid">Camera's guid</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Camera by uid</returns>
        [HttpGet("{uid:guid}")]
        public async Task<ActionResult<ApiResponseEnvelope<CameraVm>>> Get(Guid uid, CancellationToken ct = default)
        {
            var result = await _cameraService.Get(uid, ct);
            return ResponseModel(result);
        }

        /// <summary>
        /// Gets camera password for auth
        /// </summary>
        /// <returns>token</returns>
        [HttpGet("{id:guid}/password")]
        public async Task<ActionResult<ApiResponseEnvelope<string>>> GetCamerasPassword([FromQuery] Guid id, CancellationToken ct)
        {
            var result = await _cameraService.Get(id, ct);
            string base64Key = _encodingService.EncodeToBase64(result.ApiKey);
            string token = $"{id}:{base64Key}";
            return ResponseModel(token);
        }

        /// <summary>
        /// Creates a new camera object from model
        /// </summary>
        /// <param name="model">The object of CameraModel
        /// <see cref="CameraModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Newly created camera</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponseEnvelope<CameraVm>>> Create(CameraModel model, CancellationToken ct = default)
        {
            var result = await _cameraService.Create(model, ct);
            return ResponseModel(result);
        }

        /// <summary>
        /// Updates camera by id
        /// </summary>
        /// <param name="cameraGuid">Camera's guid</param>
        /// <param name="model">The object of CameraModel
        /// <see cref="CameraModel"/>
        /// </param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Camera's updated object</returns>
        [HttpPut("{cameraGuid:guid}")]
        public async Task<ActionResult<ApiResponseEnvelope<CameraVm>>> Update([FromRoute] Guid cameraGuid, CameraModel model, CancellationToken ct = default)
        {
            var result = await _cameraService.Update(cameraGuid, model, ct);
            return ResponseModel(result);
        }

        /// <summary>
        /// Patches changed fields of camera entity
        /// </summary>
        /// <param name="cameraGuid"></param>
        /// <param name="model"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPatch("{cameraGuid:guid}")]
        public async Task<ActionResult<ApiResponseEnvelope<CameraVm>>> Patch([FromRoute] Guid cameraGuid, CameraModel model, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes camera by id
        /// </summary>
        /// <param name="id">Camera's guid</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Boolean value of deletion's result</returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponseEnvelope<bool>>> Delete([FromRoute] Guid id, CancellationToken ct)
        {
            var result = await _cameraService.Delete(id, ct);
            return ResponseModel(result);
        }

    }
}
