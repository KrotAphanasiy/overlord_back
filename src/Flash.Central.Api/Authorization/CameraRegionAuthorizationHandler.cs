using System.Threading.Tasks;
using Flash.Central.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Flash.Central.Api.Authorization
{
    /// <summary>
    /// Class. Implements authorization handler for camera' regions
    /// </summary>
    public class CameraRegionAuthorizationHandler : BaseCameraHandler<CameraRegionRequirement, long>
    {

        private readonly ICameraRegionService _cameraRegionService;

        /// <summary>
        /// Constructor. Initializes the handler
        /// </summary>
        /// <param name="cameraRegionService">Defines methods bound to camera's region</param>
        public CameraRegionAuthorizationHandler(ICameraRegionService cameraRegionService)
        {
            _cameraRegionService = cameraRegionService;
        }

        /// <summary>
        /// Handles requirement
        /// </summary>
        /// <param name="context">Authorization information context</param>
        /// <param name="requirement">Authorization requirement</param>
        /// <param name="resource">Requested resource</param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CameraRegionRequirement requirement,
            long resource)
        {
            // TODO: Get from context
            var region = await _cameraRegionService.Get(resource, default);
            if (region != null)
            {
                HandleCameraRequirementAsync(context, requirement, region.CameraId);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
