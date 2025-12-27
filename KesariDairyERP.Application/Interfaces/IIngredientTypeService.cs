using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.DTOs.IngredientType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IIngredientTypeService
    {
        Task<PagedResult<IngredientTypeListDto>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? search
        );

        Task<IngredientTypeListDto> GetByIdAsync(int id);
        Task<long> CreateAsync(CreateIngredientTypeRequest request);
        Task UpdateAsync(UpdateIngredientTypeRequest request);
        Task DeleteAsync(int id);
        Task<List<IngredientTypeDropdownDto>> GetDropdownAsync();
    }
}
