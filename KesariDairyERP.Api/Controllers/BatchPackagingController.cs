using KesariDairyERP.Application.DTOs.Packaging;
using KesariDairyERP.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KesariDairyERP.Api.Controllers
{
    [ApiController]
    [Route("api/batches/{batchId}/packaging")]
    [Authorize]
    public class BatchPackagingController : ControllerBase
    {
        private readonly IBatchPackagingService _service;

        public BatchPackagingController(IBatchPackagingService service)
        {
            _service = service;
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> Calculate(
            long batchId,
            PackagingCalculateRequest request)
            => Ok(await _service.CalculateAsync(batchId, request));

        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm(
            long batchId,
            PackagingConfirmRequest request)
        {
            await _service.ConfirmAsync(batchId, request);
            return Ok();
        }
    }
}
