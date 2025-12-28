using KesariDairyERP.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<string> VerifyAsync(string verify);
        Task<string> VerifyEmailAsync(string verify);
        Task<string> VerifyUsername(string verify);
        Task <string> ChangePasswordAsync(ChangePasswordRequest changePasswordRequest);
    }
}
