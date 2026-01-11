using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.EmployeeStockAssignment
{
    public class EmployeeAssignmentDto
    {
        public DateTime Date { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
