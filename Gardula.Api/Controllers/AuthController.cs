using Gardula.Application.Authentication.DTOs;
using Gardula.Application.Authentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Gardula.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RegisterService _registerService;
    private readonly LoginService _loginService;

    public AuthController(
        RegisterService registerService,
        LoginService loginService)
    {
        _registerService = registerService;
        _loginService = loginService;
    }

    [HttpPost("register")]
    [ProducesResponseType(
        typeof(RegisterResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RegisterResponse>> Register(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _registerService.ExecuteAsync(
            request,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            response);
    }

    [HttpPost("login")]
    [ProducesResponseType(
        typeof(LoginResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _loginService.ExecuteAsync(
            request,
            cancellationToken);

        return Ok(response);
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Me()
    {
        var userId = User.FindFirstValue(
            JwtRegisteredClaimNames.Sub);

        return Ok(new
        {
            message = "Authenticated successfully.",
            userId
        });
    }
}