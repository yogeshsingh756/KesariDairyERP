using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.Inventory
{
    public class FinishedProductStockDto
    {
        public long ProductTypeId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Variant { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public int QuantityAvailable { get; set; }
        public bool IsPackaged { get; set; }
    }
}
