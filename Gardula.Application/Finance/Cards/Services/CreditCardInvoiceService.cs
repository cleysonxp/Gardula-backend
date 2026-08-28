using Gardula.Application.Common.Services;
using Gardula.Application.Finance.Cards.DTOs;
using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Cards.Services;

public class CreditCardInvoiceService
{
    private readonly ICreditCardInvoiceRepository _creditCardInvoiceRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreditCardInvoiceService(
        ICreditCardInvoiceRepository creditCardInvoiceRepository,
        ICurrentUserService currentUserService)
    {
        _creditCardInvoiceRepository = creditCardInvoiceRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<CreditCardInvoiceListItem>> GetAllAsync(
        int cardId,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var invoices = await _creditCardInvoiceRepository.GetAllByCardIdAsync(
            userId,
            cardId,
            cancellationToken);

        var response = new List<CreditCardInvoiceListItem>();

        foreach (var invoice in invoices)
        {
            var transactions =
                await _creditCardInvoiceRepository.GetTransactionsAsync(
                    userId,
                    cardId,
                    invoice.Id,
                    cancellationToken);

            var totalAmount = transactions.Sum(
                transaction => transaction.Amount);

            response.Add(
                new CreditCardInvoiceListItem(
                    invoice.Id,
                    invoice.CardId,
                    invoice.StartDate,
                    invoice.ClosingDate,
                    invoice.DueDate,
                    totalAmount,
                    (int)invoice.Status));
        }

        return response;
    }

    public async Task<List<CreditCardInvoiceListItem>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var invoices =
            await _creditCardInvoiceRepository.GetAllByUserIdAsync(
                userId,
                cancellationToken);

        var response = new List<CreditCardInvoiceListItem>();

        foreach (var invoice in invoices)
        {
            var transactions =
                await _creditCardInvoiceRepository.GetTransactionsAsync(
                    userId,
                    invoice.CardId,
                    invoice.Id,
                    cancellationToken);

            var totalAmount = transactions.Sum(
                transaction => transaction.Amount);

            response.Add(
                new CreditCardInvoiceListItem(
                    invoice.Id,
                    invoice.CardId,
                    invoice.StartDate,
                    invoice.ClosingDate,
                    invoice.DueDate,
                    totalAmount,
                    (int)invoice.Status));
        }

        return response;
    }

    public async Task<CreditCardInvoiceDetailResponse?> GetByIdAsync(
        int cardId,
        int invoiceId,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var invoice = await _creditCardInvoiceRepository.GetByIdAsync(
            userId,
            cardId,
            invoiceId,
            cancellationToken);

        if (invoice is null)
            return null;

        var transactions =
            await _creditCardInvoiceRepository.GetTransactionsAsync(
                userId,
                cardId,
                invoiceId,
                cancellationToken);

        var transactionItems = transactions
            .Select(transaction =>
                new CreditCardInvoiceTransactionItem(
                    transaction.Id,
                    transaction.Description,
                    transaction.Amount,
                    transaction.Date,
                    transaction.InstallmentNumber,
                    transaction.TotalInstallments))
            .ToList();

        var totalAmount = transactions.Sum(
            transaction => transaction.Amount);

        return new CreditCardInvoiceDetailResponse(
            invoice.Id,
            invoice.CardId,
            invoice.StartDate,
            invoice.ClosingDate,
            invoice.DueDate,
            totalAmount,
            (int)invoice.Status,
            invoice.PaidAt,
            transactionItems);
    }

    public CreditCardInvoicePeriod CalculatePeriod(
        DateTimeOffset transactionDate,
        int closingDay,
        int dueDay)
    {
        if (closingDay is < 1 or > 31)
            throw new ArgumentException(
                "Closing day must be between 1 and 31.",
                nameof(closingDay));

        if (dueDay is < 1 or > 31)
            throw new ArgumentException(
                "Due day must be between 1 and 31.",
                nameof(dueDay));

        var transactionMonth = transactionDate.Month;
        var transactionYear = transactionDate.Year;

        var closingDate = CreateDate(
            transactionYear,
            transactionMonth,
            closingDay);

        if (transactionDate.Date > closingDate.Date)
        {
            closingDate = CreateDate(
                transactionYear,
                transactionMonth,
                closingDay)
                .AddMonths(1);
        }

        var startDate = closingDate.AddMonths(-1).AddDays(1);

        var dueDate = CreateDate(
            closingDate.Year,
            closingDate.Month,
            dueDay);

        if (dueDate.Date <= closingDate.Date)
        {
            dueDate = dueDate.AddMonths(1);
        }

        return new CreditCardInvoicePeriod(
            startDate,
            closingDate,
            dueDate);
    }

    private static DateTimeOffset CreateDate(
        int year,
        int month,
        int day)
    {
        var daysInMonth = DateTime.DaysInMonth(
            year,
            month);

        var validDay = Math.Min(
            day,
            daysInMonth);

        return new DateTimeOffset(
            year,
            month,
            validDay,
            0,
            0,
            0,
            TimeSpan.Zero);
    }
}