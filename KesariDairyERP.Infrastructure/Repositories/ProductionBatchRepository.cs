using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Domain.Entities;
using KesariDairyERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Infrastructure.Repositories
{
    public class ProductionBatchRepository : IProductionBatchRepository
    {
        private readonly AppDbContext _db;

        public ProductionBatchRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(ProductionBatch batch)
        {
            _db.ProductionBatch.Add(batch);
            await _db.SaveChangesAsync();
        }
        public async Task UpdateAsync(ProductionBatch batch)
        {
            _db.ProductionBatch.Update(batch);
            await _db.SaveChangesAsync();
        }
        public void DeleteIngredients(IEnumerable<ProductionBatchIngredient> ingredients)
        {
            _db.ProductionBatchIngredient.RemoveRange(ingredients);
        }
        public async Task<PagedResult<ProductionBatch>> GetPagedAsync(
    int pageNumber, int pageSize, DateTime? batchDate)
        {
            var query = _db.ProductionBatch
       .Include(x => x.Product)   
       .Where(x => !x.IsDeleted);

            if (batchDate.HasValue)
            {
                query = query.Where(x => x.BatchDate.Date == batchDate.Value.Date);
            }

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.BatchDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ProductionBatch>
            {
                Items = items,
                TotalRecords = total
            };
        }

        public async Task<ProductionBatch> GetByIdWithIngredientsAsync(int id)
        {
            return await _db.ProductionBatch
    .Include(x => x.Product)                 // ✅ ProductType
    .Include(x => x.Ingredients)
        .ThenInclude(i => i.IngredientType)  // ✅ IngredientType
    .FirstAsync(x => x.Id == id && !x.IsDeleted);
        }
        public async Task<ProductionBatch> GetByIdAsync(long id)
        {
            return await _db.ProductionBatch
    .Include(x => x.Product)                 // ✅ ProductType
    .Include(x => x.Ingredients)
        .ThenInclude(i => i.IngredientType)  // ✅ IngredientType
    .FirstAsync(x => x.Id == id && !x.IsDeleted);
        }

    }
}
