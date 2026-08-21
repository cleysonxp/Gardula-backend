namespace Gardula.Application.Finance.Accounts.DTOs;

public record AccountBalanceItem(
    int AccountId,
    decimal CurrentBalance);