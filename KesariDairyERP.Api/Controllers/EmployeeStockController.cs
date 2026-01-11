using KesariDairyERP.Application.DTOs.EmployeeStockAssignment;
using KesariDairyERP.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KesariDairyERP.Api.Controllers
{
    [ApiController]
    [Route("api/employee-stock")]
    [Authorize]
    public class EmployeeStockController : ControllerBase
    {
        private readonly IEmployeeStockService _service;

        public EmployeeStockController(IEmployeeStockService service)
        {
            _service = service;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> Assign(
            AssignProductToEmployeeRequest request)
        {
            await _service.AssignAsync(request);
            return Ok();
        }
        [HttpGet("assignments")]
        public async Task<IActionResult> GetAssignments(
    int pageNumber = 1,
    int pageSize = 10,
    long? employeeId = null,
    DateTime? fromDate = null,
    DateTime? toDate = null)
        {
            return Ok(await _service.GetAssignmentSummaryAsync(
                pageNumber,
                pageSize,
                employeeId,
                fromDate,
                toDate));
        }

        [HttpGet("assignments/details")]
        public async Task<IActionResult> GetAssignmentDetails(
    long employeeId,
    long productTypeId,
    int pageNumber = 1,
    int pageSize = 10)
        {
            return Ok(await _service.GetAssignmentDetailsAsync(
                employeeId,
                productTypeId,
                pageNumber,
                pageSize));
        }
    }
}
