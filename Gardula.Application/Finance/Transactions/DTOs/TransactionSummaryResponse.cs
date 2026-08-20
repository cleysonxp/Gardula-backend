namespace Gardula.Application.Finance.Transactions.DTOs;

public record TransactionSummaryResponse(
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance,
    int TotalTransactions);