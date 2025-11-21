using System.Threading.Tasks;

namespace agriApp.Services.Auth
{
    public interface IOtpService
    {
        Task SendOtpAsync(string mobileNumber);
        Task<bool> VerifyOtpAsync(string mobileNumber, string otp);
    }
}
