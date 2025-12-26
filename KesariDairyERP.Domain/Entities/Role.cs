using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; } = null!;
        public string? Description { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
            = new List<RolePermission>();
    }
}
