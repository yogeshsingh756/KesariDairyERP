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

        public InventoryService(IInventoryRepository repo)
        {
            _repo = repo;
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
    }
}
