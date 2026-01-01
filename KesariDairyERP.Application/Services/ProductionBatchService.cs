using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.DTOs.ProductionBatch;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Services
{
    public class ProductionBatchService : IProductionBatchService
    {
        private readonly IProductionBatchRepository _repo;
        private readonly IInventoryRepository _inventoryRepo;

        public ProductionBatchService(IProductionBatchRepository repo, IInventoryRepository inventoryRepo)
        {
            _repo = repo;
            _inventoryRepo = inventoryRepo;
        }

        public async Task<CreateProductionBatchResponse> CreateAsync(
      CreateProductionBatchRequest request)
        {
            decimal totalIngredientCost = 0;

            var batch = new ProductionBatch
            {
                ProductId = request.ProductId,
                BatchQuantity = request.BatchQuantity,
                BatchUnit = request.BatchUnit,
                Fat = request.Fat,
                SNF = request.SNF,
                ProcessingFeePerUnit = request.ProcessingFeePerUnit,
                BatchDate = request.BatchDate,
                BasePricePerUnit = request.BasePricePerUnit
            };

            // ---------------- INGREDIENT COST CALCULATION ----------------
            foreach (var ing in request.Ingredients)
            {
                var normalizedQty = NormalizeQuantity(ing.QuantityUsed, ing.Unit);
                var ingredientCost = normalizedQty * ing.CostPerUnit;

                batch.Ingredients.Add(new ProductionBatchIngredient
                {
                    IngredientTypeId = ing.IngredientTypeId,
                    QuantityUsed = ing.QuantityUsed,
                    Unit = ing.Unit,
                    CostPerUnit = ing.CostPerUnit,
                    TotalCost = ingredientCost
                });

                totalIngredientCost += ingredientCost;
            }

            // ---------------- PROCESSING COST ----------------
            var processingCost =
                request.BatchQuantity * request.ProcessingFeePerUnit;

            var ingredientCostPerUnit =
                totalIngredientCost / request.BatchQuantity;

            // ---------------- FINAL PRICING (ORDER MATTERS) ----------------

            batch.TotalIngredientCost = totalIngredientCost;

            batch.ActualCostPerUnit =
                batch.BasePricePerUnit + ingredientCostPerUnit;

            //Selling price MUST be calculated BEFORE TotalCost
            batch.SellingPricePerUnit =
                batch.ActualCostPerUnit + request.ProcessingFeePerUnit;

            batch.TotalProcessingCost = processingCost;

            //TotalCost must use SellingPricePerUnit (already calculated)
            batch.TotalCost =
                batch.SellingPricePerUnit * request.BatchQuantity;
            // ---------------- MILK STOCK DEDUCTION ----------------

            // Milk quantity is always batch quantity (LITER)
            var requiredMilkQty = request.BatchQuantity;

            // Fetch milk stock
            var milkStock = await _inventoryRepo
                .GetByRawMaterialAsync("MILK");

            if (milkStock == null)
            {
                throw new Exception("Milk stock not found");
            }

            // ❌ Block negative stock
            if (milkStock.QuantityAvailable < requiredMilkQty)
            {
                throw new Exception(
                    $"Insufficient milk stock. Available: {milkStock.QuantityAvailable} L, Required: {requiredMilkQty} L");
            }

            // ✅ Deduct milk
            milkStock.QuantityAvailable -= requiredMilkQty;

            // Save stock update
            await _inventoryRepo.UpdateAsync(milkStock);

            // ---------------- SAVE ----------------
            await _repo.AddAsync(batch);

            // ---------------- RESPONSE ----------------
            return new CreateProductionBatchResponse
            {
                BatchId = batch.Id,
                BasePricePerUnit = batch.BasePricePerUnit,
                ActualCostPerUnit = batch.ActualCostPerUnit,
                SellingPricePerUnit = batch.SellingPricePerUnit,
                TotalIngredientCost = batch.TotalIngredientCost,
                TotalProcessingCost = batch.TotalProcessingCost,
                TotalCost = batch.TotalCost
            };
        }
        public async Task UpdateAsync(
    long batchId,
    UpdateProductionBatchRequest request)
        {
            var batch = await _repo.GetByIdWithIngredientsAsync(Convert.ToInt32(batchId))
                ?? throw new Exception("Batch not found");

            // 🔒 HARD LOCK AFTER PACKAGING
            if (batch.TotalPacketsCreated > 0)
                throw new Exception("Batch cannot be updated after packaging");

            // ❌ Product & Unit IMMUTABLE
            if (batch.ProductId != request.ProductId ||
                batch.BatchUnit != request.BatchUnit)
            {
                throw new Exception("Product or unit cannot be changed");
            }

            // ---------------- MILK STOCK ADJUSTMENT ----------------
            decimal oldQty = batch.BatchQuantity;
            decimal newQty = request.BatchQuantity;
            decimal difference = newQty - oldQty;

            if (difference != 0)
            {
                var milkStock = await _inventoryRepo
                    .GetByRawMaterialAsync("MILK")
                    ?? throw new Exception("Milk stock not found");

                if (difference > 0 && milkStock.QuantityAvailable < difference)
                {
                    throw new Exception(
                        $"Insufficient milk stock. Required extra: {difference} L");
                }

                // 🔁 ADJUST STOCK (SAFE)
                milkStock.QuantityAvailable -= difference;
                await _inventoryRepo.UpdateAsync(milkStock);
            }

            // ---------------- UPDATE BASIC FIELDS ----------------
            batch.BatchQuantity = newQty;
            batch.Fat = request.Fat;
            batch.SNF = request.SNF;
            batch.BasePricePerUnit = request.BasePricePerUnit;
            batch.ProcessingFeePerUnit = request.ProcessingFeePerUnit;
            batch.BatchDate = request.BatchDate;
            batch.UpdatedAt = DateTime.Now;

            // 🔥 FIX: Delete old ingredients first
            _repo.DeleteIngredients(batch.Ingredients);

            // ---------------- INGREDIENT RECALC ----------------
            batch.Ingredients.Clear();

            decimal totalIngredientCost = 0;

            foreach (var ing in request.Ingredients)
            {
                var normalizedQty = NormalizeQuantity(ing.QuantityUsed, ing.Unit);
                var ingredientCost = normalizedQty * ing.CostPerUnit;

                batch.Ingredients.Add(new ProductionBatchIngredient
                {
                    IngredientTypeId = ing.IngredientTypeId,
                    QuantityUsed = ing.QuantityUsed,
                    Unit = ing.Unit,
                    CostPerUnit = ing.CostPerUnit,
                    TotalCost = ingredientCost
                });

                totalIngredientCost += ingredientCost;
            }

            // ---------------- PRICE RECALC ----------------
            // ---------------- PROCESSING COST ----------------
            var processingCost =
                request.BatchQuantity * request.ProcessingFeePerUnit;

            var ingredientCostPerUnit =
                totalIngredientCost / request.BatchQuantity;

            // ---------------- FINAL PRICING (ORDER MATTERS) ----------------

            batch.TotalIngredientCost = totalIngredientCost;

            batch.ActualCostPerUnit =
                batch.BasePricePerUnit + ingredientCostPerUnit;

            //Selling price MUST be calculated BEFORE TotalCost
            batch.SellingPricePerUnit =
                batch.ActualCostPerUnit + request.ProcessingFeePerUnit;

            batch.TotalProcessingCost = processingCost;

            //TotalCost must use SellingPricePerUnit (already calculated)
            batch.TotalCost =
                batch.SellingPricePerUnit * request.BatchQuantity;

            await _repo.UpdateAsync(batch);
        }
        public async Task DeleteAsync(long batchId)
        {
            var batch = await _repo.GetByIdAsync(batchId)
                ?? throw new Exception("Batch not found");

            // 🔒 HARD LOCK AFTER PACKAGING
            if (batch.TotalPacketsCreated > 0)
                throw new Exception("Batch cannot be deleted after packaging");

            // ---------------- RETURN MILK STOCK ----------------
            var milkStock = await _inventoryRepo
                .GetByRawMaterialAsync("MILK")
                ?? throw new Exception("Milk stock not found");

            milkStock.QuantityAvailable += batch.BatchQuantity;
            await _inventoryRepo.UpdateAsync(milkStock);

            // ---------------- SOFT DELETE ----------------
            batch.IsDeleted = true;
            await _repo.UpdateAsync(batch);
        }

        private decimal NormalizeQuantity(decimal qty, string unit)
        {
            return unit.ToUpper() switch
            {
                "KG" => qty,
                "GM" => qty / 1000,
                "LITER" => qty,
                "ML" => qty / 1000,
                _ => throw new Exception("Invalid unit")
            };
        }
        public async Task<PagedResult<ProductionBatchListDto>> GetPagedAsync(
    int pageNumber, int pageSize, DateTime? batchDate)
        {
            var result = await _repo.GetPagedAsync(pageNumber, pageSize, batchDate);

            return new PagedResult<ProductionBatchListDto>
            {
                Items = result.Items.Select(b => new ProductionBatchListDto
                {
                    Id = b.Id,
                    ProductId = b.ProductId,
                    ProductName = b.Product?.Name + " " + b.Product?.Variant,
                    BatchQuantity = b.BatchQuantity,
                    BatchUnit = b.BatchUnit,
                    BasePricePerUnit = b.BasePricePerUnit,
                    ActualCostPerUnit = b.ActualCostPerUnit,
                    SellingPricePerUnit = b.SellingPricePerUnit,
                    TotalCost = b.TotalCost,
                    BatchDate = b.BatchDate,
                    TotalPacketsCreated = b.TotalPacketsCreated
                }).ToList(),
                TotalRecords = result.TotalRecords
            };
        }

        public async Task<ProductionBatchDetailDto> GetByIdAsync(int id)
        {
            var batch = await _repo.GetByIdWithIngredientsAsync(id);

            return new ProductionBatchDetailDto
            {
                Id = batch.Id,
                ProductId = batch.ProductId,
                ProductName = batch.Product?.Name + " " + batch?.Product?.Variant,
                BatchQuantity = batch.BatchQuantity,
                BatchUnit = batch.BatchUnit,
                BasePricePerUnit = batch.BasePricePerUnit,
                Fat = batch.Fat,
                SNF = batch.SNF,
                ActualCostPerUnit = batch.ActualCostPerUnit,
                SellingPricePerUnit = batch.SellingPricePerUnit,
                TotalIngredientCost = batch.TotalIngredientCost,
                TotalProcessingCost = batch.TotalProcessingCost,
                ProcessingFeePerUnit = batch.ProcessingFeePerUnit,
                TotalCost = batch.TotalCost,
                BatchDate = batch.BatchDate,
                Ingredients = batch.Ingredients.Select(i => new ProductionBatchIngredientDto
                {
                    IngredientTypeId = i.IngredientTypeId,
                    IngredientTypeName = i.IngredientType.Name,
                    QuantityUsed = i.QuantityUsed,
                    Unit = i.Unit,
                    CostPerUnit = i.CostPerUnit,
                    TotalCost = i.TotalCost
                }).ToList()
            };
        }
    }
}
