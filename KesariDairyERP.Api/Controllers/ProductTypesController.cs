using KesariDairyERP.Api.Authorization;
using KesariDairyERP.Application.DTOs.ProductType;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KesariDairyERP.Api.Controllers
{
    [ApiController]
    [Route("api/product-types")]
    [Authorize]
    public class ProductTypesController : ControllerBase
    {
        private readonly IProductTypeService _service;

        public ProductTypesController(IProductTypeService service)
        {
            _service = service;
        }
        [HttpGet]
        [HasPermission(Permissions.ProductTypeView)]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        [HasPermission(Permissions.ProductTypeView)]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        [HasPermission(Permissions.ProductTypeCreate)]
        public async Task<IActionResult> Create(CreateProductTypeRequest request)
            => Ok(await _service.CreateAsync(request));

        [HttpPut]
        [HasPermission(Permissions.ProductTypeEdit)]
        public async Task<IActionResult> Update(UpdateProductTypeRequest request)
        {
            await _service.UpdateAsync(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        [HasPermission(Permissions.RProductTypeDelete)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
