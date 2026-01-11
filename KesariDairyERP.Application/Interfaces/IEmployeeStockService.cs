using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.DTOs.EmployeeStockAssignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IEmployeeStockService
    {
        Task AssignAsync(AssignProductToEmployeeRequest request);
        Task<PagedResult<EmployeeAssignmentSummaryDto>>
         GetAssignmentSummaryAsync(
             int pageNumber,
             int pageSize,
             long? employeeId,
             DateTime? fromDate,
             DateTime? toDate);

        Task<PagedResult<EmployeeAssignmentDetailDto>>
            GetAssignmentDetailsAsync(
                long employeeId,
                long productTypeId,
                int pageNumber,
                int pageSize);
    }
}
