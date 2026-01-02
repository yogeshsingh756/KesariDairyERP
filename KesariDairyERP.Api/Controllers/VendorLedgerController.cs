using KesariDairyERP.Api.Authorization;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KesariDairyERP.Api.Controllers
{
    [ApiController]
    [Route("api/vendor-ledger")]
    [Authorize]
    public class VendorLedgerController : ControllerBase
    {
        private readonly IVendorLedgerService _service;

        public VendorLedgerController(IVendorLedgerService service)
        {
            _service = service;
        }

        [HttpGet]
        [HasPermission(Permissions.VendorsLedgersView)]
        public async Task<IActionResult> GetLedger(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? search = null,
    [FromQuery] string? vendorType = null)
        {
            var result = await _service.GetLedgerAsync(
                pageNumber,
                pageSize,
                search,
                vendorType);

            return Ok(result);
        }
        [HttpGet("vendor/{vendorId:long}/transactions")]
        [HasPermission(Permissions.VendorsLedgersView)]
        public async Task<IActionResult> GetVendorLedgerTransactions(long vendorId)
        {
            var data = await _service.GetVendorLedgerTransactionsAsync(vendorId);
            return Ok(data);
        }
    }
}
