using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.RawMaterial
{
    public class PurchaseRawMaterialDto
    {
        public long Id { get; set; }
        public long PurchaseMasterId { get; set; }

        public string RawMaterialType { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;

        public decimal RatePerUnit { get; set; }
        public decimal Amount { get; set; }
    }
}
