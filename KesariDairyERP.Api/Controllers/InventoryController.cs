using KesariDairyERP.Api.Authorization;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KesariDairyERP.Api.Controllers
{
    [ApiController]
    [Route("api/inventory")]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _service;

        public InventoryController(IInventoryService service)
        {
            _service = service;
        }

        [HttpGet]
        [HasPermission(Permissions.InventoryView)]
        public async Task<IActionResult> GetAll(int pageNumber = 1,
        int pageSize = 10,
        string? search = null)
            => Ok(await _service.GetAllAsync(pageNumber,pageSize,search));

        [HttpGet("finished-product-stock")]
        [HasPermission(Permissions.InventoryView)]
        public async Task<IActionResult> GetFinishedProductStock()
        {
            return Ok(await _service.GetFinishedProductsAsync());
        }
    }
}
