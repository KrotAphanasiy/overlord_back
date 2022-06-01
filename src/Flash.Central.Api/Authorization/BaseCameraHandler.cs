using System;
using System.Security.Claims;
using Flash.Central.Core.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Flash.Central.Api.Authorization
{
    /// <summary>
    /// Class. Base class for authorization handlers
    /// </summary>
    public abstract class BaseCameraHandler<TRequirement, TResource> : AuthorizationHandler<TRequirement, TResource> where TRequirement : IAuthorizationRequirement
    {

        /// <summary>
        /// Handles requirement
        /// </summary>
        /// <param name="context">AuthorizationHandlerContext</param>
        /// <param name="requirement">Requested requirement</param>
        /// <param name="resource">Requested resource</param>
        /// <returns></returns>
        protected void HandleCameraRequirementAsync(
            AuthorizationHandlerContext context,
            TRequirement requirement,
            Guid resource)
        {
            if (context.User.Identity is {IsAuthenticated: true})
            {
                context.Succeed(requirement);
                return;
            }

            if (context.User.IsSuperuser())
            {
                context.Succeed(requirement);
                return;
            }

            var cameraName = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAuthorized = resource.ToString() == cameraName;

            if (isAuthorized)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
