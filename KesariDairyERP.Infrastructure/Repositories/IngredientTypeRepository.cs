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
    public class IngredientTypeRepository : IIngredientTypeRepository
    {
        private readonly AppDbContext _db;

        public IngredientTypeRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<IngredientType>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? search)
        {
            var query = _db.IngredientType
                .Where(x => !x.IsDeleted);

            // 🔍 Search by Name or Unit
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(x =>
                    x.Name.ToLower().Contains(search) ||
                    x.Unit.ToLower().Contains(search)
                );
            }

            // 📊 Total count BEFORE pagination
            var total = await query.CountAsync();

            // 📄 Apply pagination
            var items = await query
                .OrderBy(x => x.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<IngredientType>
            {
                Items = items,
                TotalRecords = total
            };
        }

        public async Task<IngredientType> GetByIdAsync(int id)
        {
            return await _db.IngredientType
                .FirstAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task AddAsync(IngredientType entity)
        {
            _db.IngredientType.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(IngredientType entity)
        {
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            entity.IsDeleted = true;
            await _db.SaveChangesAsync();
        }
    }
}
