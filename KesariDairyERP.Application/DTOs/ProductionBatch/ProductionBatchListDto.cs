using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.ProductionBatch
{
    public class ProductionBatchListDto
    {
        public long Id { get; set; }
        public int ProductId { get; set; }
        public decimal BatchQuantity { get; set; }
        public string BatchUnit { get; set; } = null!;
        public decimal BasePricePerUnit { get; set; }
        public decimal ActualCostPerUnit { get; set; }
        public decimal SellingPricePerUnit { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime BatchDate { get; set; }
    }
}
