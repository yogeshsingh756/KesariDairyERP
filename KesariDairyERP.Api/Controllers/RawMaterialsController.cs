using KesariDairyERP.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KesariDairyERP.Api.Controllers
{
    [ApiController]
    [Route("api/raw-materials")]
    [Authorize]
    public class RawMaterialsController : ControllerBase
    {
        private readonly IRawMaterialService _service;

        public RawMaterialsController(IRawMaterialService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());
    }
}
