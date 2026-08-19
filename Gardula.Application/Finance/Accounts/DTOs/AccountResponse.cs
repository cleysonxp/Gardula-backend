namespace Gardula.Application.Finance.Accounts.DTOs;

public record AccountResponse(
    int Id,
    string Name,
    int Type,
    decimal InitialBalance,
    string Color,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);