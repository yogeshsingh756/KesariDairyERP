using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Domain.Entities
{
    public class BatchPackaging : BaseEntity
    {
        public long ProductionBatchId { get; set; }
        public ProductionBatch ProductionBatch { get; set; } = null!;

        public long ProductTypeId { get; set; }
        public ProductType ProductType { get; set; } = null!;

        public decimal VariantQuantity { get; set; }   // 500
        public string VariantUnit { get; set; } = null!; // ML / GM

        public int CalculatedPackets { get; set; }   
        public int ActualPackets { get; set; }      
        public int DamagedPackets { get; set; }
        public string? Remarks { get; set; }

        public decimal ExtraPerUnit { get; set; }       // 10
        public int TotalPacketsCreated { get; set; }
    }
}
