using Gardula.Application.Finance.Cards.DTOs;
using Gardula.Application.Finance.Cards.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gardula.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CardsController : ControllerBase
{
    private readonly CardService _cardService;
    private readonly CreditCardInvoiceService _creditCardInvoiceService;
    private readonly CreditCardInvoicePaymentService _creditCardInvoicePaymentService;

    public CardsController(
        CardService cardService,
        CreditCardInvoiceService creditCardInvoiceService,
        CreditCardInvoicePaymentService creditCardInvoicePaymentService)
    {
        _cardService = cardService;
        _creditCardInvoiceService = creditCardInvoiceService;
        _creditCardInvoicePaymentService = creditCardInvoicePaymentService;
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(CardResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CardResponse>> Create(
        CreateCardRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _cardService.CreateAsync(
            request,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            response);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(List<CardResponse>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<CardResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var response = await _cardService.GetAllAsync(
            cancellationToken);

        return Ok(response);
    }

    [HttpGet("overview")]
    [ProducesResponseType(
        typeof(CardOverviewResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CardOverviewResponse>> GetOverview(
        CancellationToken cancellationToken)
    {
        var response = await _cardService.GetOverviewAsync(
            cancellationToken);

        return Ok(response);
    }

    [HttpGet("invoices")]
    [ProducesResponseType(
        typeof(List<CreditCardInvoiceListItem>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<CreditCardInvoiceListItem>>> GetAllInvoices(
        CancellationToken cancellationToken)
    {
        var response = await _creditCardInvoiceService.GetAllAsync(
            cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(
        typeof(CardResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CardResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var response = await _cardService.GetByIdAsync(
            id,
            cancellationToken);

        if (response is null)
            return NotFound();

        return Ok(response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(
        typeof(CardResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CardResponse>> Update(
        int id,
        UpdateCardRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _cardService.UpdateAsync(
            id,
            request,
            cancellationToken);

        if (response is null)
            return NotFound();

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var deleted = await _cardService.DeleteAsync(
            id,
            cancellationToken);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpGet("{cardId:int}/invoices")]
    [ProducesResponseType(
        typeof(List<CreditCardInvoiceListItem>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<CreditCardInvoiceListItem>>> GetInvoices(
        int cardId,
        CancellationToken cancellationToken)
    {
        var response = await _creditCardInvoiceService.GetAllAsync(
            cardId,
            cancellationToken);

        return Ok(response);
    }

    [HttpGet("{cardId:int}/invoices/{invoiceId:int}")]
    [ProducesResponseType(
        typeof(CreditCardInvoiceDetailResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreditCardInvoiceDetailResponse>> GetInvoiceById(
        int cardId,
        int invoiceId,
        CancellationToken cancellationToken)
    {
        var response = await _creditCardInvoiceService.GetByIdAsync(
            cardId,
            invoiceId,
            cancellationToken);

        if (response is null)
            return NotFound();

        return Ok(response);
    }

    [HttpPost("{cardId:int}/invoices/{invoiceId:int}/pay")]
    [ProducesResponseType(
        typeof(CreditCardInvoiceDetailResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreditCardInvoiceDetailResponse>> PayInvoice(
        int cardId,
        int invoiceId,
        PayCreditCardInvoiceRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _creditCardInvoicePaymentService.PayAsync(
            cardId,
            invoiceId,
            request,
            cancellationToken);

        if (response is null)
            return NotFound();

        return Ok(response);
    }
}