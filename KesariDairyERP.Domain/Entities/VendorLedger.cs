using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Domain.Entities
{
    public class VendorLedger : BaseEntity
    {
        public long VendorId { get; set; }
        public Vendor Vendor { get; set; } = null!;

        public long PurchaseMasterId { get; set; }
        public PurchaseMaster PurchaseMaster { get; set; } = null!;

        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingAmount { get; set; }

        public DateTime EntryDate { get; set; }
    }
}
