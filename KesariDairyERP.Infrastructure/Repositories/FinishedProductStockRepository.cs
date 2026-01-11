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
    public class FinishedProductStockRepository : IFinishedProductStockRepository
    {
        private readonly AppDbContext _db;

        public FinishedProductStockRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<FinishedProductStock?> GetByProductTypeAsync(long productTypeId)
            => await _db.FinishedProductStock
                .FirstOrDefaultAsync(x =>
                    x.ProductTypeId == productTypeId && !x.IsDeleted);

        public async Task SaveAsync(FinishedProductStock stock)
        {
            if (stock.Id == 0)
                _db.FinishedProductStock.Add(stock);
            else
                _db.FinishedProductStock.Update(stock);

            await _db.SaveChangesAsync();
        }
        public async Task<List<FinishedProductStock>> GetAllAsync()
        {
            return await _db.FinishedProductStock
                .Include(x => x.ProductType)
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }
    }
}
