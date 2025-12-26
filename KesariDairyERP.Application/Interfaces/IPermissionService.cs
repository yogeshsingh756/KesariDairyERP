using KesariDairyERP.Application.DTOs.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IPermissionService
    {
        Task<List<PermissionGroupDto>> GetAllGroupedAsync();
    }

}
