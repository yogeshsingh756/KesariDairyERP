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

        public ProductionBatchService(IProductionBatchRepository repo)
        {
            _repo = repo;
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
                    BatchDate = b.BatchDate
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
