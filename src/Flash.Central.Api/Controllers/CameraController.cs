using System;
using System.Threading;
using System.Threading.Tasks;
using DigitalSkynet.DotnetCore.Api.Controllers;
using DigitalSkynet.DotnetCore.DataStructures.Models.Response;
using Flash.Central.Api.Authorization;
using Flash.Central.Api.Extensions;
using Flash.Central.Core.Services.Interfaces;
using Flash.Central.ViewModel.Camera;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flash.Central.Api.Controllers
{
    /// <summary>
    /// Class of the controller. Represents endpoints responsible for work with camera's datas.
    /// </summary>
    [Route("api/v1/[controller]")]
    public class CameraController : BaseApiController
    {
        private readonly ICameraService _cameraService;
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        ///Constructor. Initializes controller's parameters
        /// </summary>
        /// <param name="cameraService">Defines methods bound to camera</param>
        /// <param name="authorizationService">Defines methods bound to authorization</param>
        public CameraController(
            ICameraService cameraService,
            IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
            _cameraService = cameraService;
        }

        /// <summary>
        /// Gets camera by id
        /// </summary>
        /// <param name="uid">Camera's guid</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Camera's object by uid</returns>
        [HttpGet("{uid:guid}")]
        [Authorize]
        public async Task<ActionResult<ApiResponseEnvelope<CameraVm>>> Get(Guid uid, CancellationToken ct = default)
        {
            await _authorizationService.AuthorizeResourceAsync<Guid, CameraRequirement>(User, uid);
            var result = await _cameraService.Get(uid, ct);
            return ResponseModel(result);
        }
    }
}
