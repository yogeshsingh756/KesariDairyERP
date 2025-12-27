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
        Task<User?> GetByIdAsync(long id);
        Task CreateUserWithRoleAsync(User user, long roleId);
        Task UpdateUserAsync(UpdateUserDto dto);
        Task SoftDeleteAsync(long userId);
    }
    }
