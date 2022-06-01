using System;
using System.Security.Claims;
using Flash.Central.Foundation.Constants;
using Microsoft.AspNetCore.Identity;

namespace Flash.Central.Core.Extensions
{
    /// <summary>
    /// Class. Implements ClaimsPrincipal extension
    /// </summary>
    public static class ClaimsIdentityExtensions
    {
        /// <summary>
        /// Extension. Checks if the Superuser has claims
        /// </summary>
        /// <param name="principal">ClaimsPrincipal</param>
        /// <returns>Boolean value</returns>
        public static bool IsSuperuser(this ClaimsPrincipal principal)
        {
            return principal.HasClaim(ClaimTypes.Role, UserRoles.Superuser);
        }
    }
}
