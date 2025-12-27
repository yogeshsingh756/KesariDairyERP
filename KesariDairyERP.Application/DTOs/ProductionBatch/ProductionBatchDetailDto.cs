using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.ProductionBatch
{
    public class ProductionBatchDetailDto
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal BatchQuantity { get; set; }
        public string BatchUnit { get; set; } = null!;
        public decimal BasePricePerUnit { get; set; }
        public decimal Fat { get; set; }
        public decimal SNF { get; set; }
        public decimal ActualCostPerUnit { get; set; }
        public decimal SellingPricePerUnit { get; set; }
        public decimal TotalIngredientCost { get; set; }
        public decimal TotalProcessingCost { get; set; }
        public decimal ProcessingFeePerUnit { get; set; }   
        public decimal TotalCost { get; set; }
        public DateTime BatchDate { get; set; }

        public List<ProductionBatchIngredientDto> Ingredients { get; set; } = new();
    }
}
