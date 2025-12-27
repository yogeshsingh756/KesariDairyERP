using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.ProductionBatch
{
    public class ProductionBatchIngredientDto
    {
        public long IngredientTypeId { get; set; }
        public decimal QuantityUsed { get; set; }
        public string Unit { get; set; } = null!;
        public decimal CostPerUnit { get; set; }
        public decimal TotalCost { get; set; }
    }
}
