using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.DTOs.Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IPurchaseService
    {
        MilkPurchaseCalculateResponse CalculateMilk(
        MilkPurchaseCalculateRequest request);

        Task ConfirmMilkPurchaseAsync(
            MilkPurchaseConfirmRequest request);

        OtherMaterialPurchaseCalculateResponse CalculateOtherMaterial(
        OtherMaterialPurchaseCalculateRequest request);

        Task ConfirmOtherMaterialPurchaseAsync(
            OtherMaterialPurchaseConfirmRequest request);

        Task<PagedResult<PurchaseListDto>> GetPurchasesAsync(
    int pageNumber,
    int pageSize,
    string? rawMaterialType,
    DateTime? fromDate,
    DateTime? toDate);
    }
}
