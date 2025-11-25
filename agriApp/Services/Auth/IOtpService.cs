using System.Threading.Tasks;

namespace agriApp.Services.Auth
{
    public interface IOtpService
    {
        Task SendOtpAsync(string mobileNumber, string userPreviousId=null, bool allowCreateUser = true);
        Task<bool> VerifyOtpAsync(string mobileNumber, string otp);
    }
}
