using Gardula.Application.Common.DTOs;
using Gardula.Application.Finance.Transactions.DTOs;
using Gardula.Application.Finance.Transactions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gardula.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly TransactionService _transactionService;

    public TransactionsController(TransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(TransactionResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TransactionResponse>> Create(
        CreateTransactionRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _transactionService.CreateAsync(
            request,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            response);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(PagedResponse<TransactionListResponse>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PagedResponse<TransactionListResponse>>> GetAll(
        [FromQuery] TransactionFilterRequest filter,
        CancellationToken cancellationToken)
    {
        var response = await _transactionService.GetAllAsync(
            filter,
            cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(
        typeof(TransactionDetailResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransactionDetailResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var response = await _transactionService.GetDetailByIdAsync(
            id,
            cancellationToken);

        if (response is null)
            return NotFound();

        return Ok(response);
    }

    [HttpGet("summary")]
    [ProducesResponseType(
        typeof(TransactionSummaryResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TransactionSummaryResponse>> GetSummary(
        [FromQuery] TransactionFilterRequest filter,
        CancellationToken cancellationToken)
    {
        var response = await _transactionService.GetSummaryAsync(
            filter,
            cancellationToken);

        return Ok(response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(
        typeof(TransactionResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransactionResponse>> Update(
        int id,
        UpdateTransactionRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _transactionService.UpdateAsync(
                id,
                request,
                cancellationToken);

            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        try
        {
            await _transactionService.DeleteAsync(
                id,
                cancellationToken);

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
    }

    [HttpDelete("installments/{installmentGroupId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteInstallmentGroup(
        Guid installmentGroupId,
        CancellationToken cancellationToken)
    {
        try
        {
            await _transactionService.DeleteInstallmentGroupAsync(
                installmentGroupId,
                cancellationToken);

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("{id:int}/installment")]
    [ProducesResponseType(
    typeof(TransactionResponse),
    StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransactionResponse>> UpdateInstallment(
        int id,
        UpdateInstallmentRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _transactionService.UpdateInstallmentAsync(
                id,
                request,
                cancellationToken);

            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
    }
}