using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.DTOs.Inventory;
using KesariDairyERP.Application.DTOs.Users;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repo;
        private readonly IFinishedProductStockRepository _finishedStockRepo;

        public InventoryService(IInventoryRepository repo, IFinishedProductStockRepository finishedStockRepo)
        {
            _repo = repo;
            _finishedStockRepo = finishedStockRepo;
        }

        public async Task<PagedResult<InventoryStockDto>> GetAllAsync(int pageNumber,
        int pageSize,
        string? search)
        {
            var (inventory, total) = await _repo.GetAllAsync(
              pageNumber,
              pageSize,
              search
          );

            return new PagedResult<InventoryStockDto>
            {
                Items = inventory,
                TotalRecords = total
            };
        }
        public async Task<InventoryStock?> GetByRawMaterialAsync(string rawMaterialType)
        {
            return await _repo.GetByRawMaterialAsync(rawMaterialType);
        }
        public async Task<List<FinishedProductStockDto>> GetFinishedProductsAsync()
        {
            var stocks = await _finishedStockRepo.GetAllAsync();

            return stocks.Select(s => new FinishedProductStockDto
            {
                ProductTypeId = s.ProductTypeId,
                ProductName = s.ProductType.Name,
                Variant = s.ProductType.Variant,
                Unit = s.ProductType.Unit,
                QuantityAvailable = s.QuantityAvailable,
                IsPackaged = s.QuantityAvailable > 0
            }).ToList();
        }
    }
}
        