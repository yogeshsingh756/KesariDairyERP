using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.Purchase
{
    public class MilkPurchaseCalculateRequest
    {
        public decimal Quantity { get; set; }   // in Litre
        public decimal Fat { get; set; }        // %
        public decimal CLR { get; set; }
        public decimal Rate { get; set; }       // e.g. 6600
    }
}
