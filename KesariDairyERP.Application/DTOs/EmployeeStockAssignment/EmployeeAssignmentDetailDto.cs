using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.EmployeeStockAssignment
{
    public class EmployeeAssignmentDetailDto
    {
        public DateTime AssignmentDate { get; set; }
        public int QuantityAssigned { get; set; }

        public decimal SellingPricePerUnit { get; set; }
        public decimal TotalAmount { get; set; }

        public string? Remarks { get; set; }
    }
}
