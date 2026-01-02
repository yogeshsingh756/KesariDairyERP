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
        IQueryable<VendorLedger> GetLedgerQueryable(string? vendorType = null);
        IQueryable<VendorLedger> GetVendorLedgerQueryable();
    }
}
