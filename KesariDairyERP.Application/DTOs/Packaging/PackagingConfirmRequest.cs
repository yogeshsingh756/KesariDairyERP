using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.Packaging
{
    public class PackagingConfirmRequest
    {
        public int CalculatedPackets { get; set; }   // from Calculate API
        public int? ActualPackets { get; set; }        // user-adjusted
        public decimal ExtraPerUnit { get; set; }     // same as calculation
        public string? Remarks { get; set; }          // optional
    }
}
