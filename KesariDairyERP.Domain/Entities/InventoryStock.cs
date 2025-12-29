using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Domain.Entities
{
    public class InventoryStock : BaseEntity
    {
        public string RawMaterialType { get; set; } = null!;
        public decimal QuantityAvailable { get; set; }
    }
}
