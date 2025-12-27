using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Domain.Entities
{
    public class IngredientType : BaseEntity
    {
        public string Name { get; set; } = null!;          // Cream, Milk, Jeera
        public string Unit { get; set; } = null!;          // KG, GM, LITER
        public decimal CostPerUnit { get; set; }           // Cost per KG/LITER
        public string? Description { get; set; }
    }
}
