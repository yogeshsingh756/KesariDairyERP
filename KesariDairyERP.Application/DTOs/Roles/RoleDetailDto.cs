using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.Roles
{
    public class RoleDetailDto
    {
        public long Id { get; set; }
        public string RoleName { get; set; } = null!;
        public string? Description { get; set; }
        public List<long> PermissionIds { get; set; } = new();
    }
}
