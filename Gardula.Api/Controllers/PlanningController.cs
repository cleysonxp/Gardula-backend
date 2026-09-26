using Gardula.Application.Finance.Planning;
using Gardula.Application.Finance.Planning.DTOs;
using Gardula.Application.Finance.Planning.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gardula.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlanningController : ControllerBase
{
    private readonly MonthlyBudgetService _monthlyBudgetService;
    private readonly FinancialGoalService _financialGoalService;
    private readonly PlanningService _planningService;

    public PlanningController(
        MonthlyBudgetService monthlyBudgetService,
        FinancialGoalService financialGoalService,
        PlanningService planningService)
    {
        _monthlyBudgetService = monthlyBudgetService;
        _financialGoalService = financialGoalService;
        _planningService = planningService;
    }

    [HttpGet("overview")]
    [ProducesResponseType(
        typeof(PlanningOverviewResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlanningOverviewResponse>> GetOverview(
        [FromQuery] int year,
        [FromQuery] int month,
        CancellationToken cancellationToken)
    {
        var response = await _planningService.GetOverviewAsync(
            year,
            month,
            cancellationToken);

        if (response is null)
            return NotFound();

        return Ok(response);
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

    [HttpGet("goals")]
    [ProducesResponseType(
        typeof(List<FinancialGoalResponse>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<FinancialGoalResponse>>> GetGoals(
        CancellationToken cancellationToken)
    {
        var response = await _financialGoalService.GetAllAsync(
            cancellationToken);

        return Ok(response);
    }

    [HttpGet("goals/{id:int}")]
    [ProducesResponseType(
        typeof(FinancialGoalResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FinancialGoalResponse>> GetGoal(
        int id,
        CancellationToken cancellationToken)
    {
        var response = await _financialGoalService.GetByIdAsync(
            id,
            cancellationToken);

        if (response is null)
            return NotFound();

        return Ok(response);
    }

    [HttpPost("goals")]
    [ProducesResponseType(
        typeof(FinancialGoalResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<FinancialGoalResponse>> CreateGoal(
        CreateFinancialGoalRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _financialGoalService.CreateAsync(
            request,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            response);
    }

    [HttpPut("goals/{id:int}")]
    [ProducesResponseType(
        typeof(FinancialGoalResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FinancialGoalResponse>> UpdateGoal(
        int id,
        UpdateFinancialGoalRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _financialGoalService.UpdateAsync(
            id,
            request,
            cancellationToken);

        if (response is null)
            return NotFound();

        return Ok(response);
    }

    [HttpPost("goals/{id:int}/amount")]
    [ProducesResponseType(
        typeof(FinancialGoalResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FinancialGoalResponse>> AddGoalAmount(
        int id,
        AddFinancialGoalAmountRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _financialGoalService.AddAmountAsync(
            id,
            request.Amount,
            cancellationToken);

        if (response is null)
            return NotFound();

        return Ok(response);
    }
}