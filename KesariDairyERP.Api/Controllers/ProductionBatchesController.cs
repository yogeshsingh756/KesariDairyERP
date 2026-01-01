using KesariDairyERP.Api.Authorization;
using KesariDairyERP.Application.DTOs.ProductionBatch;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KesariDairyERP.Api.Controllers
{
    [ApiController]
    [Route("api/production-batches")]
    [Authorize]
    public class ProductionBatchesController : ControllerBase
    {
        private readonly IProductionBatchService _service;

        public ProductionBatchesController(IProductionBatchService service)
        {
            _service = service;
        }

        [HttpPost]
        [HasPermission(Permissions.ProductionBatchCreate)]
        public async Task<IActionResult> Create(
            CreateProductionBatchRequest request)
        {
            return Ok(await _service.CreateAsync(request));
        }
        [HttpGet]
        [HasPermission(Permissions.ProductionBatchView)]
        public async Task<IActionResult> GetAll(
       int pageNumber = 1,
       int pageSize = 10,
       DateTime? batchDate = null)
       => Ok(await _service.GetPagedAsync(pageNumber, pageSize, batchDate));

        [HttpGet("{id}")]
        [HasPermission(Permissions.ProductionBatchView)]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _service.GetByIdAsync(id));

        // 🔁 UPDATE BATCH
        [HttpPut("{id}")]
        [HasPermission(Permissions.ProductionBatchEdit)]
        public async Task<IActionResult> Update(
            long id,
            UpdateProductionBatchRequest request)
        {
            await _service.UpdateAsync(id, request);
            return Ok();
        }

        // 🗑️ DELETE BATCH
        [HttpDelete("{id}")]
        [HasPermission(Permissions.ProductionBatchDelete)]
        public async Task<IActionResult> Delete(long id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
