using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.Purchase
{
    public class MilkPurchaseConfirmRequest
    {
        public decimal Quantity { get; set; }
        public decimal AvgRatePerKg { get; set; }
        public decimal Amount { get; set; }
        public long? VendorId { get; set; }
        public string? VendorName { get; set; }
        public string? ContactNumber { get; set; }
        public string? VendorType { get; set; }
        public decimal PaidAmount { get; set; }
    }
}
