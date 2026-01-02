using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.DTOs.RawMaterial;
using KesariDairyERP.Application.DTOs.Vendor;
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
        private readonly IRawMaterialRepository _rwMaterialRepository;

        public VendorLedgerService(IVendorLedgerRepository repo, IRawMaterialRepository rwMaterialRepository)
        {
            _repo = repo;
            _rwMaterialRepository = rwMaterialRepository;
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
        public async Task<List<VendorLedgerTransactionDto>>
     GetVendorLedgerTransactionsAsync(long vendorId)
        {
            // 1️⃣ Get vendor ledger transactions
            var ledgerData = await _repo.GetVendorLedgerQueryable()
                .Where(x => x.VendorId == vendorId)
                .OrderByDescending(x => x.EntryDate)
                .Select(x => new VendorLedgerTransactionDto
                {
                    Id = x.Id,
                    VendorId = x.VendorId,
                    PurchaseMasterId = x.PurchaseMasterId,
                    TotalAmount = x.TotalAmount,
                    PaidAmount = x.PaidAmount,
                    PendingAmount = x.PendingAmount,
                    EntryDate = x.EntryDate,
                    purchaseRawMaterialDtos = new List<PurchaseRawMaterialDto>() // fill later
                })
                .ToListAsync();

            // 2️⃣ Get all PurchaseMasterIds
            var purchaseMasterIds = ledgerData
                .Select(x => x.PurchaseMasterId)
                .Distinct()
                .ToList();

            // 3️⃣ Fetch all raw materials in ONE call
            var rawMaterials = await _rwMaterialRepository
                .GetByPurchaseMasterIdsAsync(purchaseMasterIds);

            // 4️⃣ Map raw materials to ledger rows
            foreach (var ledger in ledgerData)
            {
                ledger.purchaseRawMaterialDtos = rawMaterials
                    .Where(r => r.PurchaseMasterId == ledger.PurchaseMasterId)
                    .Select(r => new PurchaseRawMaterialDto
                    {
                        Id = r.Id,
                        RawMaterialType = r.RawMaterialType,
                        Quantity = r.Quantity,
                        Unit = r.Unit,
                        RatePerUnit = r.RatePerUnit,
                        Amount = r.Amount
                    })
                    .ToList();
            }

            return ledgerData;
        }
    }
}
