using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Domain.Entities
{
    public class EmployeeProductStock : BaseEntity
    {
        public long EmployeeId { get; set; }
        public User Employee { get; set; } = null!;

        public long ProductTypeId { get; set; }
        public ProductType ProductType { get; set; } = null!;

        public int QuantityAvailable { get; set; }
    }
}
