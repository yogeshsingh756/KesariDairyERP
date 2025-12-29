using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Domain.Entities
{
    public class InventoryTransaction : BaseEntity
    {
        public string RawMaterialType { get; set; } = null!;
        public decimal Quantity { get; set; }
        public string TransactionType { get; set; } = null!; // PURCHASE / CONSUME
        public long ReferenceId { get; set; }
    }
}
