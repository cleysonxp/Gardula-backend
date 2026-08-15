namespace Gardula.Application.Authentication.DTOs;

public class RegisterResponse
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public DateTimeOffset CreatedAt { get; init; }
}