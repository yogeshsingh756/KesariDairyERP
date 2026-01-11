using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.EmployeeStockAssignment
{
    public class AssignProductToEmployeeRequest
    {
        public long EmployeeId { get; set; }
        public long ProductTypeId { get; set; }

        public int Quantity { get; set; }
        public decimal SellingPricePerUnit { get; set; }
        public decimal TotalAmount { get; set; }

        public DateTime AssignmentDate { get; set; }
        public string? Remarks { get; set; }
    }
}
