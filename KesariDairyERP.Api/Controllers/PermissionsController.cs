using KesariDairyERP.Api.Authorization;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KesariDairyERP.Api.Controllers
{
    [ApiController]
    [Route("api/permissions")]
    [Authorize]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _service;

        public PermissionsController(IPermissionService service)
        {
            _service = service;
        }

        [HttpGet]
        [HasPermission(Permissions.PermissionView)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllGroupedAsync());
        }
    }
}
