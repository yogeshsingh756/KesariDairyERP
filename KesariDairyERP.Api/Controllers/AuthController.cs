using KesariDairyERP.Application.DTOs.Auth;
using KesariDairyERP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KesariDairyERP.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(string verify)
        {
            var response = await _authService.VerifyAsync(verify);
            return Ok(response);
        }
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(string verify)
        {
            var response = await _authService.VerifyEmailAsync(verify);
            return Ok(response);
        }
        [HttpPost("verify-username")]
        public async Task<IActionResult> VerifyUsername(string verify)
        {
            var response = await _authService.VerifyUsername(verify);
            return Ok(response);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(
    ChangePasswordRequest request)
        {

            var response = await _authService.ChangePasswordAsync(request);

            return Ok(response);
        }
    }
}
