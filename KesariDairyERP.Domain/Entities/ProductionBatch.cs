using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Domain.Entities
{
    public class ProductionBatch : BaseEntity
    {
        public int ProductId { get; set; }

        public decimal BatchQuantity { get; set; }
        public string BatchUnit { get; set; } = null!; // KG / LITER

        public decimal Fat { get; set; }
        public decimal SNF { get; set; }

        public decimal ProcessingFeePerUnit { get; set; }

        public decimal TotalIngredientCost { get; set; }
        public decimal TotalProcessingCost { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime BatchDate { get; set; }
        public decimal BasePricePerUnit { get; set; }        // entered from UI
        public decimal ActualCostPerUnit { get; set; }      // base + ingredients
        public decimal SellingPricePerUnit { get; set; }    // actual + processing

        public ICollection<ProductionBatchIngredient> Ingredients { get; set; }
            = new List<ProductionBatchIngredient>();
    }
}
