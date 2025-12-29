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
    }
}
