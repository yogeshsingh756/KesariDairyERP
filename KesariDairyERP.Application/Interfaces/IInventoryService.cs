using KesariDairyERP.Application.DTOs.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IInventoryService
    {
        Task<List<InventoryStockDto>> GetAllAsync();
    }
}
