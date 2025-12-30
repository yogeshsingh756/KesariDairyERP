using KesariDairyERP.Application.DTOs.Inventory;
using KesariDairyERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IInventoryRepository
    {
        Task<(List<InventoryStockDto> data, int total)>GetAllAsync(int pageNumber,
        int pageSize,
        string? search);
        Task<InventoryStock?> GetByRawMaterialAsync(string rawMaterialType);
        Task UpdateAsync(InventoryStock stock);
    }
}
