using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Domain.Entities
{
    public class PurchaseMaster : BaseEntity
    {
        public DateTime PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; }

        public ICollection<PurchaseItem> Items { get; set; }
            = new List<PurchaseItem>();
    }
}
