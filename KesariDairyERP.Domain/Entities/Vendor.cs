using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Domain.Entities
{
    public class Vendor : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public string VendorType { get; set; } = null!;
    }
}
