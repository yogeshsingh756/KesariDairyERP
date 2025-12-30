using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Domain.Entities
{
    public class FinishedProductStock : BaseEntity
    {
        public long ProductTypeId { get; set; }
        public ProductType ProductType { get; set; } = null!;

        public int QuantityAvailable { get; set; }  // packets
    }
}
