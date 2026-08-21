namespace Gardula.Application.Finance.Accounts.DTOs;

public record AccountPeriodSummaryItem(
    int AccountId,
    decimal Income,
    decimal Expense);