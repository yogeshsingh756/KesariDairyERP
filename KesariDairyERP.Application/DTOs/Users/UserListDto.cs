using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.Users
{
    public class UserListDto
    {
        public long Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
