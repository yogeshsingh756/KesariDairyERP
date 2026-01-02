using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.DTOs.Vendor;
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
        Task<PagedResult<VendorLedgerDto>> GetLedgerAsync(
     int pageNumber,
     int pageSize,
     string? search,
     string? vendorType);
        Task<List<VendorLedgerTransactionDto>>
GetVendorLedgerTransactionsAsync(long vendorId);
    }

}
