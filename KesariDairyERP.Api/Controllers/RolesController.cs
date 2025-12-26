using KesariDairyERP.Api.Authorization;
using KesariDairyERP.Application.DTOs.Roles;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KesariDairyERP.Api.Controllers
{
    [ApiController]
    [Route("api/roles")]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _service;

        public RolesController(IRoleService service)
        {
            _service = service;
        }

        [HttpGet]
        [HasPermission(Permissions.RoleView)]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id:long}")]
        [HasPermission(Permissions.RoleView)]
        public async Task<IActionResult> GetById(long id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        [HasPermission(Permissions.RoleCreate)]
        public async Task<IActionResult> Create(CreateRoleDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok("Role created");
        }

        [HttpPut]
        [HasPermission(Permissions.RoleEdit)]
        public async Task<IActionResult> Update(UpdateRoleDto dto)
        {
            await _service.UpdateAsync(dto);
            return Ok("Role updated");
        }

        [HttpDelete("{id:long}")]
        [HasPermission(Permissions.RoleDelete)]
        public async Task<IActionResult> Delete(long id)
        {
            await _service.DeleteAsync(id);
            return Ok("Role deleted");
        }
    }
}
