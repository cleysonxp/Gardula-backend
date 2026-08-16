using Gardula.Application.Authentication.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Gardula.Api.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "An unhandled exception occurred.");

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred.",
            Detail = "An unexpected error occurred while processing the request."
        };

        switch (exception)
        {
            case EmailAlreadyExistsException:
                problemDetails.Status = StatusCodes.Status409Conflict;
                problemDetails.Title = "Email already exists.";
                problemDetails.Detail = exception.Message;
                break;

            case InvalidPasswordException:
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Invalid password.";
                problemDetails.Detail = exception.Message;
                break;

            case InvalidCredentialsException:
                problemDetails.Status = StatusCodes.Status401Unauthorized;
                problemDetails.Title = "Invalid credentials.";
                problemDetails.Detail = exception.Message;
                break;
        }

        httpContext.Response.StatusCode =
            problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}