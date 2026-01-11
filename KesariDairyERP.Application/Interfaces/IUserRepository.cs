using KesariDairyERP.Application.DTOs.Auth;
using KesariDairyERP.Application.DTOs.Users;
using KesariDairyERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserWithRoleAsync(string username);

        Task<(List<User> Users, int TotalCount)> GetPagedAsync(
    int pageNumber,
    int pageSize,
    string? search
);
        Task<string> GetUserAsync(string verify);
        Task<string> GetUserEmailAsync(string verify);
        Task<string> GetUserNameAsync(string verify);
        Task<string> ChangePasswordAsync(string verify, string newPassword);
        Task<User?> GetByIdAsync(long id);
        Task<List<User>> GetUsersByRoleNameAsync(string roleName);
        Task CreateUserWithRoleAsync(User user, long roleId);
        Task UpdateUserAsync(UpdateUserDto dto);
        Task SoftDeleteAsync(long userId);
    }
    }
