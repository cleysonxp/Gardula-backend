namespace Gardula.Application.Finance.Accounts.DTOs;

public record AccountOverviewItem(
    int Id,
    string Name,
    int Type,
    string Color,
    bool IsActive,
    decimal CurrentBalance,
    decimal Income,
    decimal Expense);