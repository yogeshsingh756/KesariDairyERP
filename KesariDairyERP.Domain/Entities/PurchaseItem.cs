using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Domain.Entities
{
    public class PurchaseItem : BaseEntity
    {
        public long PurchaseMasterId { get; set; }
        public PurchaseMaster PurchaseMaster { get; set; } = null!;

        public string RawMaterialType { get; set; } = null!; // MILK, SUGAR etc

        public decimal Quantity { get; set; }
        public string Unit { get; set; } = null!;            // LITER / KG
        public decimal RatePerUnit { get; set; }
        public decimal Amount { get; set; }
    }
}
