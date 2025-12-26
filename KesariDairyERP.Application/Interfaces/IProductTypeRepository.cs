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
        Task<List<ProductType>> GetAllAsync();
        Task<ProductType?> GetByIdAsync(int id);
        Task AddAsync(ProductType entity);
        Task UpdateAsync(ProductType entity);
        Task SaveChangesAsync();
    }
}
