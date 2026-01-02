using KesariDairyERP.Application.DTOs.RawMaterial;
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
    public class RawMaterialRepository : IRawMaterialRepository
    {
        private readonly AppDbContext _db;

        public RawMaterialRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<RawMaterialDto>> GetAllAsync()
        {
            return await _db.IngredientType
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Name)
                .Select(x => new RawMaterialDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Unit = x.Unit
                })
                .ToListAsync();
        }
        public async Task<List<PurchaseRawMaterialDto>>
     GetByPurchaseMasterIdsAsync(List<long> purchaseMasterIds)
        {
            return await _db.PurchaseItem
                .Where(x =>
                    purchaseMasterIds.Contains(x.PurchaseMasterId) &&
                    x.IsActive &&
                    !x.IsDeleted)
                .Select(x => new PurchaseRawMaterialDto
                {
                    Id = x.Id,
                    PurchaseMasterId = x.PurchaseMasterId,
                    RawMaterialType = x.RawMaterialType,
                    Quantity = x.Quantity,
                    Unit = x.Unit,
                    RatePerUnit = x.RatePerUnit,
                    Amount = x.Amount
                })
                .ToListAsync();
        }
    }
}
