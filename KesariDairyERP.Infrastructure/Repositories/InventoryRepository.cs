using KesariDairyERP.Application.DTOs.Inventory;
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
    public class InventoryRepository : IInventoryRepository
    {
        private readonly AppDbContext _db;

        public InventoryRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(List<InventoryStockDto> data, int total)> GetAllAsync(
    int pageNumber,
    int pageSize,
    string? search)
        {
            var query = _db.InventoryStock
                .Where(x => !x.IsDeleted);

            // 🔍 SEARCH
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();

                query = query.Where(x =>
                    x.RawMaterialType.ToLower().Contains(search));
            }

            // 📊 TOTAL COUNT (before pagination)
            var total = await query.CountAsync();

            // 📄 PAGINATION
            var data = await query
                .OrderBy(x => x.RawMaterialType)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new InventoryStockDto
                {
                    RawMaterialType = x.RawMaterialType,
                    QuantityAvailable = x.QuantityAvailable
                })
                .ToListAsync();

            return (data, total);
        }
        public async Task<InventoryStock?> GetByRawMaterialAsync(string rawMaterialType)
        {
            return await _db.InventoryStock
                .FirstOrDefaultAsync(x =>
                    x.RawMaterialType == rawMaterialType &&
                    !x.IsDeleted);
        }

        public async Task UpdateAsync(InventoryStock stock)
        {
            _db.InventoryStock.Update(stock);
            await _db.SaveChangesAsync();
        }
    }
}
