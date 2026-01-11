using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Domain.Entities
{
    public class EmployeeProductAssignment : BaseEntity
    {
        public long EmployeeId { get; set; }
        public User Employee { get; set; } = null!;

        public long ProductTypeId { get; set; }
        public ProductType ProductType { get; set; } = null!;
        public decimal SellingPricePerUnit { get; set; }
        public decimal TotalAmount { get; set; }
        public int QuantityAssigned { get; set; }
        public DateTime AssignmentDate { get; set; }
        public string? Remarks { get; set; }
    }
}
