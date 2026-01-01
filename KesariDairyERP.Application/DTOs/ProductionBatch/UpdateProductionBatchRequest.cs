using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.ProductionBatch
{
    public class UpdateProductionBatchRequest
    {
        public long ProductId { get; set; }
        public decimal BatchQuantity { get; set; }
        public string BatchUnit { get; set; } = null!;

        public decimal Fat { get; set; }
        public decimal SNF { get; set; }

        public decimal ProcessingFeePerUnit { get; set; }
        public DateTime BatchDate { get; set; }
        public decimal BasePricePerUnit { get; set; }

        public List<CreateProductionBatchIngredientRequest> Ingredients { get; set; }
            = new();
    }
}
