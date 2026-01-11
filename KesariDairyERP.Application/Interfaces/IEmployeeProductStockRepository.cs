using KesariDairyERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IEmployeeProductStockRepository
    {
        Task<EmployeeProductStock?> GetAsync(long employeeId, long productTypeId);
        Task AddAsync(EmployeeProductStock stock);
        Task UpdateAsync(EmployeeProductStock stock);
    }
}
