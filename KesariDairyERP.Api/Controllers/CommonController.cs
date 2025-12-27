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
    }
}
