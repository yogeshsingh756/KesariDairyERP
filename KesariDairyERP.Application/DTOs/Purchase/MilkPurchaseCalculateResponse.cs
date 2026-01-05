using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.Purchase
{
    public class MilkPurchaseCalculateResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        public decimal SNFPercent { get; set; }
        public decimal FatKg { get; set; }
        public decimal SNFKg { get; set; }
        public decimal AvgRatePerKg { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
