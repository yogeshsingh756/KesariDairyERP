using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.Auth
{
    public class ChangePasswordRequest
    {
        public string Verify { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
