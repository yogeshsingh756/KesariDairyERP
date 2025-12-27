using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.DTOs.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IRoleService
    {
        Task<PagedResult<RoleListDto>> GetPagedAsync(
    int pageNumber,
    int pageSize,
    string? search
);
        Task<RoleDetailDto> GetByIdAsync(long id);
        Task CreateAsync(CreateRoleDto dto);
        Task UpdateAsync(UpdateRoleDto dto);
        Task DeleteAsync(long id);
    }
}
