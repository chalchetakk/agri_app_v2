using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace agriApp.Extensions
{
    public static class ClaimsExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        }

        public static Guid GetOfficialId(this ClaimsPrincipal user)
        {
            var val = user.FindFirstValue("officialId");
            return val == null ? Guid.Empty : Guid.Parse(val);
        }

        public static int GetMandiId(this ClaimsPrincipal user)
        {
            var val = user.FindFirstValue("mandiId");
            return val == null ? 0 : int.Parse(val);
        }

        public static string GetOfficialRole(this ClaimsPrincipal user)
        {
            return user.FindFirstValue("officialRole") ?? "";
        }
    }
}
