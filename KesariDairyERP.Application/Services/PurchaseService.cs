using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.DTOs.Purchase;
using KesariDairyERP.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _repo;

        public PurchaseService(IPurchaseRepository repo)
        {
            _repo = repo;
        }


        public MilkPurchaseCalculateResponse CalculateMilk(
        MilkPurchaseCalculateRequest request)
        {
            // SNF
            var snfRaw = (request.CLR / 4m) + (0.21m * request.Fat) + 0.10m;
            var snf = Math.Floor(snfRaw * 100m) / 100m;

            // Kg
            var fatKg = Math.Round(request.Quantity * request.Fat / 100m, 2);
            var snfKg = Math.Round(request.Quantity * snf / 100m, 2);

            //// Rates
            var rawRate = request.Rate / 100m;      // 53.00
            var adjustedRate = rawRate + 0.20m;     // 53.20

            // ---- FAT RATE (RAW -> CEILING ONLY HERE) ----
            decimal fatRateRaw = rawRate * (request.Fat / 29.3m);
            decimal fatRateFinal = Math.Ceiling(fatRateRaw * 100m) / 100m;

            // ---- AVG RATE (RAW) ----
            decimal avgRateRaw = adjustedRate * (snf / 10m);
            decimal avgRateFinal = Math.Round(avgRateRaw, 2, MidpointRounding.AwayFromZero);

            // ---- TOTAL AMOUNT (RAW avgRate -> CEILING) ----
            decimal totalAmountRaw = avgRateRaw * request.Quantity;
            decimal totalAmountFinal = Math.Ceiling(totalAmountRaw * 100m) / 100m;
 
            return new MilkPurchaseCalculateResponse
            {
                SNFPercent = snf,
                FatKg = fatKg,
                SNFKg = snfKg,
                FatRate = fatRateFinal,
                AvgRatePerKg = avgRateFinal,
                TotalAmount = totalAmountFinal
            };

        }

        // STEP 2: CONFIRM & SAVE
        public async Task ConfirmMilkPurchaseAsync(
            MilkPurchaseConfirmRequest request)
        {
            await _repo.SaveMilkPurchaseAsync(request);
        }

        public OtherMaterialPurchaseCalculateResponse CalculateOtherMaterial(
        OtherMaterialPurchaseCalculateRequest request)
        {
            var amount = request.Quantity * request.RatePerUnit;

            return new OtherMaterialPurchaseCalculateResponse
            {
                Amount = Math.Round(amount, 2)
            };
        }

        public async Task ConfirmOtherMaterialPurchaseAsync(
            OtherMaterialPurchaseConfirmRequest request)
        {
            await _repo.SaveOtherMaterialPurchaseAsync(request);
        }

        public async Task<PagedResult<PurchaseListDto>> GetPurchasesAsync(
    int pageNumber,
    int pageSize,
    string? rawMaterialType,
    DateTime? fromDate,
    DateTime? toDate)
        {
            var query = _repo.QueryPurchases();

            if (!string.IsNullOrWhiteSpace(rawMaterialType))
                query = query.Where(x => x.RawMaterialType == rawMaterialType);

            if (fromDate.HasValue)
                query = query.Where(x => x.PurchaseDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(x => x.PurchaseDate <= toDate.Value);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.PurchaseDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new PurchaseListDto
                {
                    PurchaseId = x.PurchaseId,
                    PurchaseDate = x.PurchaseDate,
                    RawMaterialType = x.RawMaterialType,
                    Quantity = x.Quantity,
                    Unit = x.Unit,
                    RatePerUnit = x.RatePerUnit,
                    Amount = x.Amount
                })
                .ToListAsync();

            return new PagedResult<PurchaseListDto>
            {
                Items = items,
                TotalRecords = total
            };
        }
    }
}
