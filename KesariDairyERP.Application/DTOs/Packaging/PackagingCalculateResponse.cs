using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.Packaging
{
    public class PackagingCalculateResponse
    {
        public decimal BatchQuantityBaseUnit { get; set; }
        public decimal VariantQuantityBaseUnit { get; set; }
        public decimal ExtraPerUnit { get; set; }
        public decimal PerPacketConsumption { get; set; }
        public int TotalPackets { get; set; }
        public decimal Wastage { get; set; }
    }
}
