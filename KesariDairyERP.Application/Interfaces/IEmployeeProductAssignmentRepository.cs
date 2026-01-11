using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IEmployeeProductAssignmentRepository
    {
        Task AddAsync(EmployeeProductAssignment assignment);
        IQueryable<EmployeeProductAssignment> Query();

        IQueryable<EmployeeProductAssignment>
            GetDetailsQuery(long employeeId, long productTypeId);
    }
}
