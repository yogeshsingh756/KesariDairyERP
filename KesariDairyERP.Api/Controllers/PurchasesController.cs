using KesariDairyERP.Api.Authorization;
using KesariDairyERP.Application.DTOs.Purchase;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KesariDairyERP.Api.Controllers
{
    [ApiController]
    [Route("api/purchases")]
    [Authorize]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseService _service;

        public PurchasesController(IPurchaseService service)
        {
            _service = service;
        }

        [HttpPost("milk/calculate")]
        [HasPermission(Permissions.PurchasesCreate)]
        public IActionResult CalculateMilk(
            MilkPurchaseCalculateRequest request)
        {
            return Ok(_service.CalculateMilk(request));
        }

        [HttpPost("milk/confirm")]
        [HasPermission(Permissions.PurchasesCreate)]
        public async Task<IActionResult> ConfirmMilk(
            MilkPurchaseConfirmRequest request)
        {
            await _service.ConfirmMilkPurchaseAsync(request);
            return Ok();
        }

        [HttpPost("other/calculate")]
        [HasPermission(Permissions.PurchasesCreate)]
        public IActionResult CalculateOtherMaterial(
       OtherMaterialPurchaseCalculateRequest request)
        {
            return Ok(_service.CalculateOtherMaterial(request));
        }

        [HttpPost("other/confirm")]
        [HasPermission(Permissions.PurchasesCreate)]
        public async Task<IActionResult> ConfirmOtherMaterial(
            OtherMaterialPurchaseConfirmRequest request)
        {
            await _service.ConfirmOtherMaterialPurchaseAsync(request);
            return Ok();
        }

        [HttpGet("purchases")]
        [HasPermission(Permissions.PurchasesView)]
        public async Task<IActionResult> GetPurchases(
    int pageNumber = 1,
    int pageSize = 10,
    string? rawMaterialType = null,
    DateTime? fromDate = null,
    DateTime? toDate = null)
        {
            return Ok(await _service.GetPurchasesAsync(
                pageNumber, pageSize, rawMaterialType, fromDate, toDate));
        }

    }
}
