namespace Gardula.Application.Finance.Accounts.DTOs;

public record UpdateAccountRequest(
    string Name,
    int Type,
    string Color);