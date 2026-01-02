using KesariDairyERP.Application.DTOs.RawMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.Vendor
{
    public class VendorLedgerTransactionDto
    {
        public long Id { get; set; }
        public long VendorId { get; set; }
        public long PurchaseMasterId { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingAmount { get; set; }

        public DateTime EntryDate { get; set; }
        public List<PurchaseRawMaterialDto> purchaseRawMaterialDtos { get; set; } = new List<PurchaseRawMaterialDto>();
    }
}
