using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.ProductionBatch
{
    public class CreateProductionBatchResponse
    {
        public long BatchId { get; set; }
        public decimal TotalIngredientCost { get; set; }
        public decimal TotalProcessingCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal CostPerUnit { get; set; }
        public decimal BasePricePerUnit { get; set; }        // entered from UI
        public decimal ActualCostPerUnit { get; set; }      // base + ingredients
        public decimal SellingPricePerUnit { get; set; }
    }
}
