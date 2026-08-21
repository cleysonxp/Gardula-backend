namespace Gardula.Application.Finance.Transactions.DTOs;

public record UpdateInstallmentRequest(
    decimal Amount,
    string Description,
    DateTimeOffset Date,
    int CategoryId);