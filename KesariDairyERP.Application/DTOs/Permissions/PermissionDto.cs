using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.Permissions
{
    public class PermissionDto
    {
        public long Id { get; set; }
        public string PermissionKey { get; set; } = null!;
        public string PermissionName { get; set; } = null!;
        public string ModuleName { get; set; } = null!;
    }
}
