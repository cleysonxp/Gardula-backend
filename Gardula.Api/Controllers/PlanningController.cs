using Gardula.Application.Finance.Planning.DTOs;
using Gardula.Application.Finance.Planning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gardula.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlanningController : ControllerBase
{
    private readonly MonthlyBudgetService _monthlyBudgetService;

    public PlanningController(
        MonthlyBudgetService monthlyBudgetService)
    {
        _monthlyBudgetService = monthlyBudgetService;
    }

    [HttpGet("budget")]
    [ProducesResponseType(
        typeof(MonthlyBudgetResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MonthlyBudgetResponse>> GetBudget(
        [FromQuery] int year,
        [FromQuery] int month,
        CancellationToken cancellationToken)
    {
        var response = await _monthlyBudgetService.GetAsync(
            year,
            month,
            cancellationToken);

        if (response is null)
            return NotFound();

        return Ok(response);
    }

    [HttpPost("budget")]
    [ProducesResponseType(
        typeof(MonthlyBudgetResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<MonthlyBudgetResponse>> CreateBudget(
        [FromQuery] int year,
        [FromQuery] int month,
        CreateMonthlyBudgetRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _monthlyBudgetService.CreateAsync(
            year,
            month,
            request,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            response);
    }

    [HttpPut("budget")]
    [ProducesResponseType(
        typeof(MonthlyBudgetResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MonthlyBudgetResponse>> UpdateBudget(
        [FromQuery] int year,
        [FromQuery] int month,
        UpdateMonthlyBudgetRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _monthlyBudgetService.UpdateAsync(
            year,
            month,
            request,
            cancellationToken);

        if (response is null)
            return NotFound();

        return Ok(response);
    }
}