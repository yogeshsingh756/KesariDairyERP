using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? MobileNumber { get; set; }
        public string PasswordHash { get; set; } = null!;

        // One role per user
        public UserRole UserRole { get; set; } = null!;
    }
}
