using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.Purchase
{
    public class OtherMaterialPurchaseCalculateRequest
    {
        public string RawMaterialType { get; set; } = null!; // SUGAR, CREAM
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = null!;            // KG / GM / LITER
        public decimal RatePerUnit { get; set; }
    }
}
