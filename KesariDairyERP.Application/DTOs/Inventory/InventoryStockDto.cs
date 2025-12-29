using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.Inventory
{
    public class InventoryStockDto
    {
        public string RawMaterialType { get; set; } = null!;
        public decimal QuantityAvailable { get; set; }
    }
}
