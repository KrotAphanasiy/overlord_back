using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace Flash.Central.Api.Authorization
{
    /// <summary>
    /// Class. Implements authorization handler for cameras
    /// </summary>
    public class CameraAuthorizationHandler : BaseCameraHandler<CameraRequirement, Guid>
    {
        /// <summary>
        /// Handles requirement
        /// </summary>
        /// <param name="context">Authorization information context</param>
        /// <param name="requirement">Authorization requirement</param>
        /// <param name="resource">Requested resource</param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CameraRequirement requirement,
            Guid resource)
        {
            HandleCameraRequirementAsync(context, requirement, resource);
            return Task.CompletedTask;
        }
    }
}
