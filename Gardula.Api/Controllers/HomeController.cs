using Gardula.Application.Finance.Home.DTOs;
using Gardula.Application.Finance.Home.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gardula.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HomeController : ControllerBase
{
    private readonly HomeService _homeService;

    public HomeController(HomeService homeService)
    {
        _homeService = homeService;
    }

    [HttpGet("overview")]
    [ProducesResponseType(
        typeof(HomeOverviewResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<HomeOverviewResponse>> GetOverview(
        [FromQuery] int year,
        [FromQuery] int month,
        CancellationToken cancellationToken)
    {
        var response = await _homeService.GetOverviewAsync(
            year,
            month,
            cancellationToken);

        return Ok(response);
    }
}