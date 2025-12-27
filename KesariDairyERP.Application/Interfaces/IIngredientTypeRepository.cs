using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IIngredientTypeRepository
    {
        Task<PagedResult<IngredientType>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? search
        );

        Task<IngredientType> GetByIdAsync(int id);
        Task AddAsync(IngredientType entity);
        Task UpdateAsync(IngredientType entity);
        Task DeleteAsync(int id);
    }
}
