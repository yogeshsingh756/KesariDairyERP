using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IVendorRepository
    {
        Task<List<Vendor>> GetAllAsync(string? vendorType = null);
        Task<PagedResult<Vendor>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search,
        string? vendorType);

        Task<Vendor?> GetByIdAsync(long id);
        Task AddAsync(Vendor vendor);
        Task UpdateAsync(Vendor vendor);
        Task DeleteAsync(Vendor vendor);
    }
}
