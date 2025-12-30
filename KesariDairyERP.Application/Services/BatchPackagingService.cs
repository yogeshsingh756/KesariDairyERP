using KesariDairyERP.Application.DTOs.Packaging;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Services
{
    public class BatchPackagingService : IBatchPackagingService
    {
        private readonly IProductionBatchRepository _batchRepo;
        private readonly IBatchPackagingRepository _packagingRepo;
        private readonly IFinishedProductStockRepository _stockRepo;

        public BatchPackagingService(
            IProductionBatchRepository batchRepo,
            IBatchPackagingRepository packagingRepo,
            IFinishedProductStockRepository stockRepo)
        {
            _batchRepo = batchRepo;
            _packagingRepo = packagingRepo;
            _stockRepo = stockRepo;
        }

        public async Task<PackagingCalculateResponse> CalculateAsync(
     long batchId,
     PackagingCalculateRequest request)
        {
            var batch = await _batchRepo.GetByIdAsync(batchId);

            // ---------------- 1️⃣ Normalize Batch Quantity ----------------
            // Batch unit decides BASE UNIT
            decimal totalBatchQtyBaseUnit = batch.BatchUnit.ToUpper() switch
            {
                "LITER" => batch.BatchQuantity * 1000m, // → ML
                "KG" => batch.BatchQuantity * 1000m, // → GM
                _ => throw new Exception("Batch unit must be LITER or KG")
            };

            // ---------------- 2️⃣ Extract Variant (NO UNIT DEPENDENCY) ----------------
            // Examples:
            // 500  -> 500 ml / gm
            // 1000 -> 1000 ml / gm
            // 1    -> 1000 ml / gm
            // 6    -> 6000 gm
            decimal variantValue = ExtractNumeric(batch.Product.Variant);

            decimal variantQtyBaseUnit =
                variantValue < 10
                    ? variantValue * 1000m   // 1 → 1000, 6 → 6000
                    : variantValue;          // 100, 500, 1000 stay same

            // ---------------- 3️⃣ Extra Per Packet ----------------
            decimal extraPerUnit = request.ExtraPerUnit; // always ML / GM

            decimal perPacketConsumption =
                variantQtyBaseUnit + extraPerUnit;

            // ---------------- 4️⃣ Packet Calculation ----------------
            int totalPackets = (int)Math.Floor(
                totalBatchQtyBaseUnit / perPacketConsumption
            );

            decimal usedQuantity =
                totalPackets * perPacketConsumption;

            decimal wastage =
                totalBatchQtyBaseUnit - usedQuantity;

            // ---------------- 5️⃣ Response ----------------
            return new PackagingCalculateResponse
            {
                BatchQuantityBaseUnit = totalBatchQtyBaseUnit,
                VariantQuantityBaseUnit = variantQtyBaseUnit,
                ExtraPerUnit = extraPerUnit,
                PerPacketConsumption = perPacketConsumption,
                TotalPackets = totalPackets,
                Wastage = wastage
            };
        }
        public async Task ConfirmAsync(long batchId, PackagingConfirmRequest request)
        {
            if (await _packagingRepo.ExistsAsync(batchId))
                throw new Exception("Batch already packaged");

            var batch = await _batchRepo.GetByIdAsync(batchId);
            if (batch.Product == null)
                throw new Exception("Product not loaded for batch");

            int calculatedPackets = request.CalculatedPackets;

            int actualPackets =
                request.ActualPackets > 0
                    ? request.ActualPackets.Value
                    : calculatedPackets;

            if (actualPackets <= 0)
                throw new Exception("Actual packets must be greater than zero");

            int damagedPackets =
                Math.Max(0, calculatedPackets - actualPackets);

            await _packagingRepo.AddAsync(new BatchPackaging
            {
                ProductionBatchId = batchId,
                ProductTypeId = batch.ProductId,

                VariantQuantity = batch.Product.Quantity,
                VariantUnit = batch.Product.Unit,
                ExtraPerUnit = request.ExtraPerUnit,

                CalculatedPackets = calculatedPackets,
                ActualPackets = actualPackets,
                DamagedPackets = damagedPackets,
                TotalPacketsCreated = actualPackets,

                Remarks = request.Remarks
            });

            var stock = await _stockRepo.GetByProductTypeAsync(batch.ProductId)
                ?? new FinishedProductStock
                {
                    ProductTypeId = batch.ProductId,
                    QuantityAvailable = 0
                };

            stock.QuantityAvailable += actualPackets;

            await _stockRepo.SaveAsync(stock);
        }
        private decimal ExtractNumeric(string value)
        {
            var number = new string(value.Where(char.IsDigit).ToArray());
            return decimal.Parse(number);
        }
    }
}
