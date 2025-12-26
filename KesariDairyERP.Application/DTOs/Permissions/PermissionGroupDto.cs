using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.Permissions
{
    public class PermissionGroupDto
    {
        public string ModuleName { get; set; } = null!;
        public List<PermissionDto> Permissions { get; set; } = new();
    }
}
