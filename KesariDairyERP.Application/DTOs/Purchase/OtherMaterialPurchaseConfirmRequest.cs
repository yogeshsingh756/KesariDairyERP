using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.Purchase
{
    public class OtherMaterialPurchaseConfirmRequest
    {
        public string RawMaterialType { get; set; } = null!;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = null!;
        public decimal RatePerUnit { get; set; }
        public decimal Amount { get; set; }
    }
}
