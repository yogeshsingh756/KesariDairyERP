using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.EmployeeStockAssignment
{
    public class EmployeeAssignmentSummaryDto
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; } = null!;
        public long ProductTypeId { get; set; }
        public string ProductName { get; set; } = null!;
        public int TotalQuantityAssigned { get; set; }
        public DateTime LastAssignedDate { get; set; }

        public long LastAssignmentId { get; set; } // 🔥 internal ordering
    }
}
