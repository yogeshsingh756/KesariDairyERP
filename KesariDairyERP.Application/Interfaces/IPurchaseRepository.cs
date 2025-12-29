using KesariDairyERP.Application.DTOs.Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IPurchaseRepository
    {
        Task SaveMilkPurchaseAsync(MilkPurchaseConfirmRequest request);

        Task SaveOtherMaterialPurchaseAsync(
            OtherMaterialPurchaseConfirmRequest request);

        IQueryable<PurchaseListProjection> QueryPurchases();
    }
}
