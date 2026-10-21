using Gardula.Application.Finance.Reports.DTOs;
using Gardula.Application.Finance.Reports.Services;
using Gardula.Application.Finance.Transactions.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gardula.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly ReportService _reportService;

    public ReportsController(ReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(ReportResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ReportResponse>> GetReport(
        [FromQuery] TransactionFilterRequest filter,
        CancellationToken cancellationToken)
    {
        var response = await _reportService.GetReportAsync(
            filter,
            cancellationToken);

        return Ok(response);
    }
}