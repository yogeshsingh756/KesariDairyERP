using KesariDairyERP.Application.Interfaces;
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
        public async Task<IActionResult> GetLedger()
            => Ok(await _service.GetLedgerAsync());
    }
}
