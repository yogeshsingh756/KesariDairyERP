using KesariDairyERP.Api.Authorization;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KesariDairyERP.Api.Controllers
{
    [ApiController]
    [Route("api/common")]
    [Authorize]
    public class CommonController : ControllerBase
    {
        private readonly IDashboardService _service;

        public CommonController(IDashboardService service)
        {
            _service = service;
        }
        [HttpGet("units")]
        public IActionResult GetUnits()
        {
            return Ok(new[]
            {
            new { code = "KG", label = "Kilogram" },
            new { code = "GM", label = "Gram" },
            new { code = "LITER", label = "Liter" },
            new { code = "ML", label = "Milliliter" }
        });
        }
        [HttpGet("stats")]
        [HasPermission(Permissions.DashboardView)]
        public async Task<IActionResult> GetStats()
            => Ok(await _service.GetStatsAsync());
    }
}
