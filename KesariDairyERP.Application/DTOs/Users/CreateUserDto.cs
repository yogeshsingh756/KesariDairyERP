using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.Users
{
    public class CreateUserDto
    {
        public string FullName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? MobileNumber { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public long RoleId { get; set; }
    }
}
