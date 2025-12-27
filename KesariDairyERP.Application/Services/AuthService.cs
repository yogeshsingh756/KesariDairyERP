using KesariDairyERP.Application.DTOs.Auth;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Domain.Entities;
using KesariDairyERP.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IPermissionRepository _permissionRepo;
        private readonly IConfiguration _config;

        public AuthService(
            IUserRepository userRepo,
            IPermissionRepository permissionRepo,
            IConfiguration config)
        {
            _userRepo = userRepo;
            _permissionRepo = permissionRepo;
            _config = config;
        }
        public async Task<string> VerifyAsync(string verify)
        {
            return await _userRepo.GetUserAsync(verify);
        }
        public async Task<string> ChangePasswordAsync(ChangePasswordRequest changePasswordRequest)
        {
            return await _userRepo.ChangePasswordAsync(changePasswordRequest.Verify,changePasswordRequest.NewPassword);
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepo.GetUserWithRoleAsync(request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password");

            var roleName = user.UserRole.Role.RoleName;

            // ✅ SuperAdmin bypass
            var permissions = roleName == "SuperAdmin"
                ? await _permissionRepo.GetAllPermissionKeysAsync()
                : user.UserRole.Role.RolePermissions
                    .Select(rp => rp.Permission.PermissionKey)
                    .ToList();

            var token = GenerateJwtToken(user, roleName, permissions);

            return new LoginResponse
            {
                Token = token,
                Role = roleName,
                Permissions = permissions
            };
        }

        private string GenerateJwtToken(User user, string role, List<string> permissions)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtClaims.Username, user.Username),
            new Claim(JwtClaims.Role, role)
        };

            claims.AddRange(permissions.Select(p =>
                new Claim(JwtClaims.Permission, p)));

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_config["Jwt:ExpiryMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
