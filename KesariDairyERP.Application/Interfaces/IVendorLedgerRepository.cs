using KesariDairyERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IVendorLedgerRepository
    {
        Task<List<VendorLedger>> GetLedgerAsync(string? vendorType = null);
    }
}
