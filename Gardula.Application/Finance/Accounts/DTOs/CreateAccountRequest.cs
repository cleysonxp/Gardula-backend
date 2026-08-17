namespace Gardula.Application.Finance.Accounts.DTOs;

public record CreateAccountRequest(
    string Name,
    int Type,
    decimal InitialBalance);