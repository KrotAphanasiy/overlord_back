using System.Security.Claims;
using System.Threading.Tasks;
using DigitalSkynet.DotnetCore.DataStructures.Exceptions.Api;
using Microsoft.AspNetCore.Authorization;

namespace Flash.Central.Api.Extensions
{
    /// <summary>
    /// Class. Represents authorization extensions
    /// </summary>
    public static class AuthorizationExtensions
    {

        /// <summary>
        /// Authorizes resource.
        /// </summary>
        /// <param name="service">Extension of IAuthorizationService. Defines mechanisms to check policy based permissions for a user</param>
        /// <param name="user">Implements claims-based identities</param>
        /// <param name="resource">Requested resource</param>
        /// <typeparam name="TResource"></typeparam>
        /// <typeparam name="TRequirement"></typeparam>
        /// <returns></returns>
        public static async Task AuthorizeResourceAsync<TResource, TRequirement>(this IAuthorizationService service, ClaimsPrincipal user, TResource resource)
            where TRequirement : IAuthorizationRequirement, new()
        {
            var authorizationResult = await service.AuthorizeAsync(user, resource, new TRequirement());
            if (!authorizationResult.Succeeded)
            {
                throw new ApiNotAuthorizedException("The required resource request is not authorized");
            }
        }
    }
}
