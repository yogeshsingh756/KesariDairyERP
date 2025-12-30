using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.DTOs.VendorLedger;
using KesariDairyERP.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Services
{
    public class VendorLedgerService : IVendorLedgerService
    {
        private readonly IVendorLedgerRepository _repo;

        public VendorLedgerService(IVendorLedgerRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<VendorLedgerDto>> GetLedgerAsync(
     int pageNumber,
     int pageSize,
     string? search,
     string? vendorType)
        {
            var query = _repo.GetLedgerQueryable(vendorType);

            // 🔍 SEARCH
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();

                query = query.Where(x =>
                    x.Vendor.Name.ToLower().Contains(search) ||
                    x.Vendor.VendorType.ToLower().Contains(search));
            }

            // 🔗 GROUPING
            var groupedQuery = query
                .GroupBy(x => new
                {
                    x.VendorId,
                    x.Vendor.Name,
                    x.Vendor.VendorType
                })
                .Select(g => new VendorLedgerDto
                {
                    VendorId = g.Key.VendorId,
                    VendorName = g.Key.Name,
                    VendorType = g.Key.VendorType,
                    TotalAmount = g.Sum(x => x.TotalAmount),
                    PaidAmount = g.Sum(x => x.PaidAmount),
                    PendingAmount = g.Sum(x => x.PendingAmount)
                });

            // 📊 TOTAL COUNT
            var total = await groupedQuery.CountAsync();

            // 📄 PAGINATION
            var data = await groupedQuery
                .OrderBy(x => x.VendorName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<VendorLedgerDto>
            {
                Items = data,
                TotalRecords = total
            };
        }
    }
}
