using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.ProductionBatch
{
    public class CreateProductionBatchIngredientRequest
    {
        public int IngredientTypeId { get; set; }
        public decimal QuantityUsed { get; set; }
        public string Unit { get; set; } = null!;
        public decimal CostPerUnit { get; set; }
    }
}
