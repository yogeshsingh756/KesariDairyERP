using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.DTOs.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IVendorService
    {
        Task<List<VendorDto>> GetAllAsync(string? vendorType = null);
        Task<PagedResult<VendorDto>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search,
        string? vendorType);

        Task<VendorDto> GetByIdAsync(long id);
        Task CreateAsync(VendorRequest request);
        Task UpdateAsync(long id, VendorRequest request);
        Task DeleteAsync(long id);
    }
}
