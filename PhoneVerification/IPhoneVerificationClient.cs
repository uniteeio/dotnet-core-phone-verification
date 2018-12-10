using System.Threading.Tasks;
using PhoneVerification.Models;

namespace PhoneVerification
{
    public interface IPhoneVerificationClient
    {
        Task<SendCodeResponse> SendCode(string phoneNumber, int countryCode);
        Task<VerifyCodeResponse> VerifyCode(string phoneNumber, string verificationCode, int countryCode);
    }
}