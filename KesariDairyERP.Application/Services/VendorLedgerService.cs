using KesariDairyERP.Application.DTOs.VendorLedger;
using KesariDairyERP.Application.Interfaces;
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

        public async Task<List<VendorLedgerDto>> GetLedgerAsync(string? vendorType = null)
        {
            var ledgers = await _repo.GetLedgerAsync(vendorType);

            return ledgers
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
                })
                .ToList();
        }
    }
}
