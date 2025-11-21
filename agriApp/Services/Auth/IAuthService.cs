using System.Threading.Tasks;

namespace agriApp.Services.Auth
{
    public interface IAuthService
    {
        Task<LoginResult> LoginWithOtpAsync(
            string mobileNumber,
            string otp,
            string deviceInfo,
            string ipAddress);
    }
}
