using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IProductTypeRepository
    {
        Task<PagedResult<ProductType>> GetPagedAsync(
     int pageNumber,
     int pageSize,
     string? search
 );
        Task<ProductType?> GetByIdAsync(int id);
        Task AddAsync(ProductType entity);
        Task UpdateAsync(ProductType entity);
        Task SaveChangesAsync();
        Task<List<ProductType>> GetDropdownAsync();
    }
}
