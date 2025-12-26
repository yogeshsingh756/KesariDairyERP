using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public string Role { get; set; } = null!;
        public List<string> Permissions { get; set; } = new();
    }
}
