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
    }
}
