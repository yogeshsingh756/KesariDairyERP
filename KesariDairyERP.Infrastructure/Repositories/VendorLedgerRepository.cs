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
    public class VendorLedgerRepository : IVendorLedgerRepository
    {
        private readonly AppDbContext _db;

        public VendorLedgerRepository(AppDbContext db)
        {
            _db = db;
        }

        public IQueryable<VendorLedger> GetLedgerQueryable(string? vendorType = null)
        {
            var query = _db.VendorLedger
                .Include(x => x.Vendor)
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(vendorType))
            {
                query = query.Where(x => x.Vendor.VendorType == vendorType);
            }

            return query;
        }
    }
}
