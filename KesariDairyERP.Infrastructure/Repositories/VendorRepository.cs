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
    }
}
