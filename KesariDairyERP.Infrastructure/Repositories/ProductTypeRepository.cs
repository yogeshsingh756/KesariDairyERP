using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Domain.Entities;
using KesariDairyERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KesariDairyERP.Infrastructure.Repositories
{
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly AppDbContext _db;

        public ProductTypeRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<ProductType>> GetPagedAsync(
     int pageNumber,
     int pageSize,
     string? search)
        {
            var query = _db.ProductType
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(x =>
                    x.Name.ToLower().Contains(search) ||
                    x.Variant.ToLower().Contains(search)
                );
            }

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ProductType>
            {
                Items = items,
                TotalRecords = total
            };
        }

        public async Task<ProductType?> GetByIdAsync(int id)
        {
            return await _db.ProductType
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task AddAsync(ProductType entity)
        {
            await _db.ProductType.AddAsync(entity);
        }

        public Task UpdateAsync(ProductType entity)
        {
            _db.ProductType.Update(entity);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
