using KesariDairyERP.Application.DTOs.Purchase;
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
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly AppDbContext _db;

        public PurchaseRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task SaveMilkPurchaseAsync(
     MilkPurchaseConfirmRequest request)
        {
            
            // 1️⃣ Purchase Master
            var purchase = new PurchaseMaster
            {
                PurchaseDate = DateTime.UtcNow,
                TotalAmount = request.Amount
            };

            _db.PurchaseMaster.Add(purchase);
            await _db.SaveChangesAsync();
            // 1️⃣ Get or Create Vendor
            Vendor vendor;

            if (request.VendorId.HasValue)
            {
                vendor = await _db.Vendors.FindAsync(request.VendorId.Value)
                         ?? throw new Exception("Vendor not found");
            }
            else
            {
                vendor = new Vendor
                {
                    Name = request.VendorName,
                    ContactNumber = request.ContactNumber,
                    VendorType = request.VendorType
                };
                _db.Vendors.Add(vendor);
                await _db.SaveChangesAsync();
            }

            // 2️⃣ Vendor Ledger Entry
            _db.VendorLedger.Add(new VendorLedger
            {
                VendorId = vendor.Id,
                PurchaseMasterId = purchase.Id,
                TotalAmount = request.Amount,
                PaidAmount = request.PaidAmount,
                PendingAmount = request.Amount - request.PaidAmount,
                EntryDate = DateTime.UtcNow
            });

            // 2️⃣ Purchase Item
            _db.PurchaseItem.Add(new PurchaseItem
            {
                PurchaseMasterId = purchase.Id,
                RawMaterialType = "MILK",
                Quantity = request.Quantity,
                Unit = "LITER", // OK for milk
                RatePerUnit = request.AvgRatePerKg,
                Amount = request.Amount
            });

            // 3️⃣ Inventory Stock
            var stock = await _db.InventoryStock
                .FirstOrDefaultAsync(x => x.RawMaterialType == "MILK");

            if (stock == null)
            {
                stock = new InventoryStock
                {
                    RawMaterialType = "MILK",
                    QuantityAvailable = 0
                };
                _db.InventoryStock.Add(stock);
            }

            stock.QuantityAvailable += request.Quantity;

            // 🔴 4️⃣ INVENTORY TRANSACTION (MISSING EARLIER)
            _db.InventoryTransaction.Add(new InventoryTransaction
            {
                RawMaterialType = "MILK",
                Quantity = request.Quantity,
                TransactionType = "PURCHASE",
                ReferenceId = purchase.Id
            });

            await _db.SaveChangesAsync();
        }

        public async Task SaveOtherMaterialPurchaseAsync(
        OtherMaterialPurchaseConfirmRequest request)
        {
            // 1️⃣ Purchase Master
            var purchase = new PurchaseMaster
            {
                PurchaseDate = DateTime.UtcNow,
                TotalAmount = request.Amount
            };

            _db.PurchaseMaster.Add(purchase);
            await _db.SaveChangesAsync();

            // 2️⃣ Purchase Item
            _db.PurchaseItem.Add(new PurchaseItem
            {
                PurchaseMasterId = purchase.Id,
                RawMaterialType = request.RawMaterialType,
                Quantity = request.Quantity,
                Unit = request.Unit,
                RatePerUnit = request.RatePerUnit,
                Amount = request.Amount
            });

            // 3️⃣ Inventory Update
            var stock = await _db.InventoryStock
                .FirstOrDefaultAsync(x =>
                    x.RawMaterialType == request.RawMaterialType);

            if (stock == null)
            {
                stock = new InventoryStock
                {
                    RawMaterialType = request.RawMaterialType,
                    QuantityAvailable = 0
                };
                _db.InventoryStock.Add(stock);
            }

            stock.QuantityAvailable += request.Quantity;

            // 4️⃣ Inventory Transaction
            _db.InventoryTransaction.Add(new InventoryTransaction
            {
                RawMaterialType = request.RawMaterialType,
                Quantity = request.Quantity,
                TransactionType = "PURCHASE",
                ReferenceId = purchase.Id
            });

            await _db.SaveChangesAsync();
        }

        public IQueryable<PurchaseListProjection> QueryPurchases()
        {
            return _db.PurchaseItem
                .Where(x => !x.IsDeleted)
                .Select(x => new PurchaseListProjection
                {
                    PurchaseId = x.PurchaseMasterId,
                    PurchaseDate = x.PurchaseMaster.PurchaseDate,
                    RawMaterialType = x.RawMaterialType,
                    Quantity = x.Quantity,
                    Unit = x.Unit,
                    RatePerUnit = x.RatePerUnit,
                    Amount = x.Amount
                });
        }
    }
}
