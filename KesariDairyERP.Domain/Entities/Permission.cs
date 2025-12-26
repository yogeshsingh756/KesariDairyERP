using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Domain.Entities
{
    public class Permission : BaseEntity
    {
        public string PermissionKey { get; set; } = null!;
        public string PermissionName { get; set; } = null!;
        public string ModuleName { get; set; } = null!;
    }
}
