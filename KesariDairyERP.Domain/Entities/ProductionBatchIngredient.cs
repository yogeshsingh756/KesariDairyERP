using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Domain.Entities
{
    public class ProductionBatchIngredient : BaseEntity
    {
        public long ProductionBatchId { get; set; }
        public ProductionBatch ProductionBatch { get; set; } = null!; 

        public long IngredientTypeId { get; set; }
        public IngredientType IngredientType { get; set; } = null!; 

        public decimal QuantityUsed { get; set; }
        public string Unit { get; set; } = null!;
        public decimal CostPerUnit { get; set; }
        public decimal TotalCost { get; set; }
    }
}
