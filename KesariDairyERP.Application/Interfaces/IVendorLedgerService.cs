using KesariDairyERP.Application.DTOs.VendorLedger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IVendorLedgerService
    {
        Task<List<VendorLedgerDto>> GetLedgerAsync(string? vendorType = null);
    }
}
