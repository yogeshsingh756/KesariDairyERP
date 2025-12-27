using KesariDairyERP.Api.Authorization;
using KesariDairyERP.Application.DTOs.IngredientType;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KesariDairyERP.Api.Controllers
{
    [ApiController]
    [Route("api/ingredient-types")]
    [Authorize]
    public class IngredientTypesController : ControllerBase
    {
        private readonly IIngredientTypeService _service;

        public IngredientTypesController(IIngredientTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        [HasPermission(Permissions.IngredientTypeView)]
        public async Task<IActionResult> GetAll(
            int pageNumber = 1,
            int pageSize = 10,
            string? search = null)
            => Ok(await _service.GetPagedAsync(pageNumber, pageSize, search));

        [HttpGet("{id}")]
        [HasPermission(Permissions.IngredientTypeView)]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        [HasPermission(Permissions.IngredientTypeCreate)]
        public async Task<IActionResult> Create(CreateIngredientTypeRequest request)
            => Ok(await _service.CreateAsync(request));

        [HttpPut]
        [HasPermission(Permissions.IngredientTypeEdit)]
        public async Task<IActionResult> Update(UpdateIngredientTypeRequest request)
        {
            await _service.UpdateAsync(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        [HasPermission(Permissions.IngredientTypeDelete)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
        [HttpGet("dropdown")]
        [HasPermission(Permissions.IngredientTypeView)]
        public async Task<IActionResult> GetDropdown()
        {
            return Ok(await _service.GetDropdownAsync());
        }
    }
}
