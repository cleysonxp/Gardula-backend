using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Cards.Services;

public class CreditCardInvoiceService
{
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