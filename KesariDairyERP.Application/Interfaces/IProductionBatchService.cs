using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.DTOs.ProductionBatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IProductionBatchService
    {
        Task<CreateProductionBatchResponse> CreateAsync(
            CreateProductionBatchRequest request);
        Task<PagedResult<ProductionBatchListDto>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        DateTime? batchDate
    );

        Task<ProductionBatchDetailDto> GetByIdAsync(int id);
        Task UpdateAsync(long batchId, UpdateProductionBatchRequest request);
        Task DeleteAsync(long batchId);
    }
}
