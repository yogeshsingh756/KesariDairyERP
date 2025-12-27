using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.DTOs.Users;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<PagedResult<UserListDto>> GetUsersAsync(
    int pageNumber,
    int pageSize,
    string? search)
        {
            var (users, total) = await _userRepo.GetPagedAsync(
                pageNumber,
                pageSize,
                search
            );

            return new PagedResult<UserListDto>
            {
                Items = users.Select(u => new UserListDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Username = u.Username,
                    Email = u.Email,
                    Role = u.UserRole.Role.RoleName,
                    IsActive = u.IsActive,
                    MobileNumber = u.MobileNumber
                }).ToList(),
                TotalRecords = total
            };
        }

        public async Task CreateUserAsync(CreateUserDto dto)
        {
            var user = new User
            {
                FullName = dto.FullName,
                Username = dto.Username,
                Email = dto.Email,
                Gender = dto.Gender,
                Address = dto.Address,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _userRepo.CreateUserWithRoleAsync(user, dto.RoleId);
        }
        public async Task UpdateUserAsync(UpdateUserDto dto)
        {
            await _userRepo.UpdateUserAsync(dto);
        }

        public async Task SoftDeleteUserAsync(long userId)
        {
            await _userRepo.SoftDeleteAsync(userId);
        }
        public async Task<UserDetailDto> GetByIdAsync(long id)
        {
            var user = await _userRepo.GetByIdAsync(id);

            if (user == null)
                throw new Exception("User not found");

            return new UserDetailDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Username = user.Username,
                Email = user.Email,
                Gender = user.Gender,
                Address = user.Address,
                IsActive = user.IsActive,
                RoleId = user.UserRole.RoleId,
                RoleName = user.UserRole.Role.RoleName
            };
        }
    }
}
