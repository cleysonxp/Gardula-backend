namespace Gardula.Application.Finance.Cards.DTOs;

public record PayCreditCardInvoiceRequest(
    int AccountId,
    int PaymentMethod);