using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Domain.Entities
{
    public class ProductType : BaseEntity
    {

        public string Name { get; set; } = null!;       // Milk, Curd
        public string Variant { get; set; } = null!;    // 500 ml, 400 g
        public string Unit { get; set; } = null!;       // ml, g, kg
        public decimal Quantity { get; set; }           // 500, 400, Numeric value of the unit 
    }
}
