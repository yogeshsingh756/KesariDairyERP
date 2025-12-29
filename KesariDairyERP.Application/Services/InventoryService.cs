using KesariDairyERP.Application.DTOs.Inventory;
using KesariDairyERP.Application.Interfaces;
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

        public async Task<List<InventoryStockDto>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
    }
}
