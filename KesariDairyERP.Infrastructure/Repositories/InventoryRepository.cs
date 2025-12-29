using KesariDairyERP.Application.DTOs.Inventory;
using KesariDairyERP.Application.Interfaces;
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

        public async Task<List<InventoryStockDto>> GetAllAsync()
        {
            return await _db.InventoryStock
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.RawMaterialType)
                .Select(x => new InventoryStockDto
                {
                    RawMaterialType = x.RawMaterialType,
                    QuantityAvailable = x.QuantityAvailable
                })
                .ToListAsync();
        }
    }
}
