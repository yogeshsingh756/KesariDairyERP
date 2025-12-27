using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IUserService
    {
        Task<PagedResult<UserListDto>> GetUsersAsync(
    int pageNumber,
    int pageSize,
    string? search
);
        Task<UserDetailDto> GetByIdAsync(long id);
        Task CreateUserAsync(CreateUserDto dto);
        Task UpdateUserAsync(UpdateUserDto dto);
        Task SoftDeleteUserAsync(long userId);
    }
}
