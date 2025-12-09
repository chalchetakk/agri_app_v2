using System;
using System.Threading.Tasks;
using agriApp.Entities.Auth;

namespace agriApp.Services.Auth
{
    public interface ITokenService
    {
        string GenerateAccessToken(UserProfile user, string deviceInfo, string ipAddress, string? roleCode, Guid? officialId, int? mandiId);

        Task<string> GenerateAndStoreRefreshTokenAsync(
            Guid userId,
            string deviceInfo,
            string ipAddress,
            string accessTokenJti);

        Task<JwtToken?> ValidateRefreshTokenAsync(string refreshTokenRaw);

        Task RevokeRefreshTokenAsync(Guid jwtTokenId);

        Task<(string accessToken, string refreshToken)> RotateRefreshTokenAsync(string oldRefreshTokenRaw);
    }
}
