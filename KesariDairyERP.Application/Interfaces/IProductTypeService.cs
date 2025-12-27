using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.DTOs.ProductType;
using KesariDairyERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IProductTypeService
    {
        Task<PagedResult<ProductType>> GetPagedAsync(
    int pageNumber,
    int pageSize,
    string? search
);
        Task<ProductType> GetByIdAsync(int id);
        Task<long> CreateAsync(CreateProductTypeRequest request);
        Task UpdateAsync(UpdateProductTypeRequest request);
        Task DeleteAsync(int id);
        Task<List<ProductDropdownDto>> GetDropdownAsync();
    }
}
