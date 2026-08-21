using Gardula.Application.Finance.Transactions.DTOs;

namespace Gardula.Application.Finance.Accounts.DTOs;

public record AccountOverviewResponse(
    decimal TotalBalance,
    int ActiveAccounts,
    decimal TotalIncome,
    decimal TotalExpense,
    List<AccountOverviewItem> Accounts,
    List<TransactionListResponse> RecentTransactions);