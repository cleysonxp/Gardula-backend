using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Gardula.Application.Authentication.DTOs;
using Gardula.Application.Authentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gardula.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RegisterService _registerService;
    private readonly LoginService _loginService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ILogoutService _logoutService;

    public AuthController(
        RegisterService registerService,
        LoginService loginService,
        IRefreshTokenService refreshTokenService,
        ILogoutService logoutService)
    {
        _registerService = registerService;
        _loginService = loginService;
        _refreshTokenService = refreshTokenService;
        _logoutService = logoutService;
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

    [HttpPost("refresh")]
    [ProducesResponseType(
        typeof(LoginResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Refresh(
        RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _refreshTokenService.ExecuteAsync(
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

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout(
    LogoutRequest request,
    CancellationToken cancellationToken)
    {
        await _logoutService.ExecuteAsync(
            request,
            cancellationToken);

        return NoContent();
    }
}