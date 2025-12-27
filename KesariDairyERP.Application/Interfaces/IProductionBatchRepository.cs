using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IProductionBatchRepository
    {
        Task AddAsync(ProductionBatch batch);
        Task<PagedResult<ProductionBatch>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        DateTime? batchDate
    );

        Task<ProductionBatch> GetByIdWithIngredientsAsync(int id);
    }
}
