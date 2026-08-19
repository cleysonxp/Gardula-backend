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

    public CardsController(CardService cardService)
    {
        _cardService = cardService;
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
}