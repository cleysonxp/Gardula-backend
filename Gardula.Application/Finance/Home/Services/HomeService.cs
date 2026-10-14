using Gardula.Application.Finance.Accounts.DTOs;
using Gardula.Application.Finance.Accounts.Services;
using Gardula.Application.Finance.Cards.DTOs;
using Gardula.Application.Finance.Cards.Services;
using Gardula.Application.Finance.Home.DTOs;
using Gardula.Application.Finance.Planning.DTOs;
using Gardula.Application.Finance.Planning.Services;
using Gardula.Application.Finance.Transactions.DTOs;
using Gardula.Application.Finance.Transactions.Services;
using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Home.Services;

public class HomeService
{
    private readonly AccountService _accountService;
    private readonly TransactionService _transactionService;
    private readonly PlanningService _planningService;
    private readonly FinancialGoalService _financialGoalService;
    private readonly CardService _cardService;
    private readonly CreditCardInvoiceService _creditCardInvoiceService;

    public HomeService(
        AccountService accountService,
        TransactionService transactionService,
        PlanningService planningService,
        FinancialGoalService financialGoalService,
        CardService cardService,
        CreditCardInvoiceService creditCardInvoiceService)
    {
        _accountService = accountService;
        _transactionService = transactionService;
        _planningService = planningService;
        _financialGoalService = financialGoalService;
        _cardService = cardService;
        _creditCardInvoiceService = creditCardInvoiceService;
    }

    public async Task<HomeOverviewResponse> GetOverviewAsync(
        int year,
        int month,
        CancellationToken cancellationToken = default)
    {
        if (month < 1 || month > 12)
            throw new ArgumentOutOfRangeException(nameof(month));

        if (year < 1 || year > 9999)
            throw new ArgumentOutOfRangeException(nameof(year));

        var startDate = new DateTimeOffset(
            year,
            month,
            1,
            0,
            0,
            0,
            TimeSpan.Zero);

        var endDate = startDate
            .AddMonths(1)
            .AddTicks(-1);

        var accountOverview = await _accountService.GetOverviewAsync(
            new AccountOverviewFilterRequest(
                StartDate: startDate,
                EndDate: endDate),
            cancellationToken);

        var monthlySummary = await _transactionService.GetSummaryAsync(
            new TransactionFilterRequest(
                StartDate: startDate,
                EndDate: endDate),
            cancellationToken);

        var planningOverview = await _planningService.GetOverviewAsync(
            year,
            month,
            cancellationToken);

        var goals = await _financialGoalService.GetAllAsync(
            cancellationToken);

        var cardOverview = await _cardService.GetOverviewAsync(
            cancellationToken);

        var cards = await _cardService.GetAllAsync(
            cancellationToken);

        var usedLimits = await _cardService.GetUsedLimitsAsync(
            cancellationToken);

        var cardItems = new List<HomeCardResponse>();

        foreach (var card in cards)
        {
            var usedLimit = usedLimits
                .FirstOrDefault(item => item.CardId == card.Id)
                ?.UsedLimit ?? 0m;

            var invoices = await _creditCardInvoiceService.GetAllAsync(
                card.Id,
                cancellationToken);

            var currentInvoice = invoices
                .Where(invoice =>
                    invoice.Status != (int)CreditCardInvoiceStatus.Paid)
                .OrderBy(invoice =>
                    invoice.Status == (int)CreditCardInvoiceStatus.Overdue
                        ? 0
                        : 1)
                .ThenBy(invoice => invoice.DueDate)
                .FirstOrDefault();

            cardItems.Add(
                new HomeCardResponse(
                    card.Id,
                    card.Name,
                    card.LastFourDigits,
                    card.CreditLimit,
                    usedLimit,
                    card.CreditLimit - usedLimit,
                    currentInvoice?.TotalAmount,
                    currentInvoice?.DueDate,
                    currentInvoice?.Status));
        }

        var budget = planningOverview?.Budget
            ?? new BudgetOverviewResponse(
                0m,
                0m,
                0m,
                0m,
                false);

        var categorySpending = planningOverview?.CategorySpending
            ?? new List<CategorySpendingResponse>();

        return new HomeOverviewResponse(
            year,
            month,
            accountOverview.TotalBalance,
            monthlySummary,
            budget,
            categorySpending,
            goals,
            cardOverview,
            cardItems,
            accountOverview.RecentTransactions);
    }
}