using KesariDairyERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IFinishedProductStockRepository
    {
        Task<FinishedProductStock?> GetByProductTypeAsync(long productTypeId);
        Task SaveAsync(FinishedProductStock stock);
        Task<List<FinishedProductStock>> GetAllAsync();
    }
}
