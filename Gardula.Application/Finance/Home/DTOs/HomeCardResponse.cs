namespace Gardula.Application.Finance.Home.DTOs;

public record HomeCardResponse(
    int Id,
    string Name,
    string LastFourDigits,
    decimal CreditLimit,
    decimal UsedLimit,
    decimal AvailableLimit,
    decimal? InvoiceAmount,
    DateTimeOffset? InvoiceDueDate,
    int? InvoiceStatus);