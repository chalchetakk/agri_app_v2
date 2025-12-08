using System;
using System.Security.Claims;

namespace agriApp.Extensions
{
    public static class UserExtensions
    {
        /// <summary>
        /// Returns the authenticated user's GUID from JWT "sub" claim.
        /// </summary>
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var idString = user.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? user.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(idString))
                throw new Exception("User ID claim missing in token.");

            return Guid.Parse(idString);
        }
    }
}
