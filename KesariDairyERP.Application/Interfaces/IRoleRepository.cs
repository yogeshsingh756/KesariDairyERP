using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IRoleRepository
    {
        Task<PagedResult<Role>> GetPagedAsync(
    int pageNumber,
    int pageSize,
    string? search
);
        Task<Role?> GetByIdWithPermissionsAsync(long id);
        Task CreateAsync(Role role, List<long> permissionIds);
        Task UpdateAsync(Role role, List<long> permissionIds);
        Task DeleteAsync(long id);
    }
}
