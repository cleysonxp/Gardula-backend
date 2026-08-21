using Gardula.Application.Common.DTOs;
using Gardula.Application.Finance.Transactions.DTOs;

namespace Gardula.Application.Finance.Accounts.DTOs;

public record AccountDetailOverviewResponse(
    AccountResponse Account,
    decimal CurrentBalance,
    TransactionSummaryResponse Summary,
    PagedResponse<TransactionListResponse> Transactions);