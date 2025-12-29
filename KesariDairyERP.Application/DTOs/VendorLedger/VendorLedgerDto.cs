using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.VendorLedger
{
    public class VendorLedgerDto
    {
        public long VendorId { get; set; }
        public string VendorName { get; set; } = null!;
        public string VendorType { get; set; } = null!;

        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingAmount { get; set; }
    }
}
