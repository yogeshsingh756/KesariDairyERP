using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.ProductType
{
    public class CreateProductTypeRequest
    {
        public string Name { get; set; } = null!;
        public string Variant { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public decimal Quantity { get; set; }
    }
}
