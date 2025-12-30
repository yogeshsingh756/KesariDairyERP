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
    public class VendorRepository : IVendorRepository
    {
        private readonly AppDbContext _db;

        public VendorRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Vendor>> GetAllAsync(string? vendorType = null)
        {
            var query = _db.Vendors
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(vendorType))
            {
                query = query.Where(x => x.VendorType == vendorType);
            }

            return await query
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
        public async Task<PagedResult<Vendor>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search,
        string? vendorType)
        {
            var query = _db.Set<Vendor>().Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.Name.Contains(search) ||
                    x.ContactNumber.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(vendorType))
            {
                query = query.Where(x => x.VendorType == vendorType);
            }

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Vendor>
            {
                Items = items,
                TotalRecords = total
            };
        }

        public async Task<Vendor?> GetByIdAsync(long id)
            => await _db.Set<Vendor>()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        public async Task AddAsync(Vendor vendor)
        {
            _db.Add(vendor);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Vendor vendor)
        {
            _db.Update(vendor);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Vendor vendor)
        {
            vendor.IsDeleted = true;
            await _db.SaveChangesAsync();
        }
    }
}
