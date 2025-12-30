using KesariDairyERP.Api.Authorization;
using KesariDairyERP.Application.DTOs.Vendor;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KesariDairyERP.Api.Controllers
{
    [ApiController]
    [Route("api/vendors")]
    [Authorize]
    public class VendorsController : ControllerBase
    {
        private readonly IVendorService _service;

        public VendorsController(IVendorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("GetPagedAll")]
        [HasPermission(Permissions.VendorsView)]
        public async Task<IActionResult> GetAll(
        int pageNumber = 1,
        int pageSize = 10,
        string? search = null,
        string? vendorType = null)
        => Ok(await _service.GetPagedAsync(
            pageNumber, pageSize, search, vendorType));

        [HttpGet("{id}")]
        [HasPermission(Permissions.VendorsView)]
        public async Task<IActionResult> GetById(long id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        [HasPermission(Permissions.VendorsCreate)]
        public async Task<IActionResult> Create(VendorRequest request)
        {
            await _service.CreateAsync(request);
            return Ok();
        }

        [HttpPut("{id}")]
        [HasPermission(Permissions.VendorsEdit)]
        public async Task<IActionResult> Update(long id, VendorRequest request)
        {
            await _service.UpdateAsync(id, request);
            return Ok();
        }

        [HttpDelete("{id}")]
        [HasPermission(Permissions.VendorsDelete)]
        public async Task<IActionResult> Delete(long id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
