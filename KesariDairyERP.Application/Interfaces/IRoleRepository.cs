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
        Task<List<Role>> GetAllAsync();
        Task<Role?> GetByIdWithPermissionsAsync(long id);
        Task CreateAsync(Role role, List<long> permissionIds);
        Task UpdateAsync(Role role, List<long> permissionIds);
        Task DeleteAsync(long id);
    }
}
