using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.Users
{
    public class UpdateUserDto
    {
        public long Id { get; set; }

        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Gender { get; set; }
        public string? Address { get; set; }

        public bool IsActive { get; set; }

        public long RoleId { get; set; }
    }
}
