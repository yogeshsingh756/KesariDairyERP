using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.IngredientType
{
    public class IngredientTypeDropdownDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public decimal CostPerUnit { get; set; }
    }
}
