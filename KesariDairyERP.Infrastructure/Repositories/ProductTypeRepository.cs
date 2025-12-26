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

        public async Task<List<ProductType>> GetAllAsync()
        {
            return await _db.ProductType
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Name)
                .ToListAsync();
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
