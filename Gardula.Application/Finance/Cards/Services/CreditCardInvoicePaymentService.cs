using Gardula.Application.Common.Services;
using Gardula.Application.Finance.Accounts.Services;
using Gardula.Application.Finance.Cards.DTOs;
using Gardula.Application.Finance.Transactions.Services;
using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Cards.Services;

public class CreditCardInvoicePaymentService
{
    private readonly ICreditCardInvoiceRepository _invoiceRepository;
    private readonly ICreditCardInvoicePaymentRepository _paymentRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly CreditCardInvoiceService _invoiceService;
    private readonly ICurrentUserService _currentUserService;

    public CreditCardInvoicePaymentService(
        ICreditCardInvoiceRepository invoiceRepository,
        ICreditCardInvoicePaymentRepository paymentRepository,
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository,
        CreditCardInvoiceService invoiceService,
        ICurrentUserService currentUserService)
    {
        _invoiceRepository = invoiceRepository;
        _paymentRepository = paymentRepository;
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _invoiceService = invoiceService;
        _currentUserService = currentUserService;
    }

    public async Task<CreditCardInvoiceDetailResponse?> PayAsync(
        int cardId,
        int invoiceId,
        PayCreditCardInvoiceRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var invoice = await _invoiceRepository.GetByIdAsync(
            userId,
            cardId,
            invoiceId,
            cancellationToken);

        if (invoice is null)
            return null;

        if (invoice.Status == CreditCardInvoiceStatus.Paid)
            throw new InvalidOperationException(
                "Credit card invoice is already paid.");

        var existingPayment = await _paymentRepository.GetByInvoiceIdAsync(
            userId,
            invoiceId,
            cancellationToken);

        if (existingPayment is not null)
            throw new InvalidOperationException(
                "Credit card invoice already has a payment.");

        if (!Enum.IsDefined(
                typeof(CreditCardInvoicePaymentMethod),
                request.PaymentMethod))
        {
            throw new ArgumentException(
                "Invalid payment method.",
                nameof(request.PaymentMethod));
        }

        var paymentMethod =
            (CreditCardInvoicePaymentMethod)request.PaymentMethod;

        var account = await _accountRepository.GetByIdAsync(
            request.AccountId,
            userId,
            cancellationToken);

        if (account is null)
            throw new ArgumentException(
                "Account not found.",
                nameof(request.AccountId));

        if (!account.IsActive)
            throw new ArgumentException(
                "Account is inactive.",
                nameof(request.AccountId));

        var transactions = await _invoiceRepository.GetTransactionsAsync(
            userId,
            cardId,
            invoiceId,
            cancellationToken);

        var totalAmount = transactions.Sum(
            transaction => transaction.Amount);

        if (totalAmount <= 0)
            throw new InvalidOperationException(
                "Credit card invoice has no amount to pay.");

        var accountBalance = await _accountRepository.GetBalanceByAccountIdAsync(
            request.AccountId,
            userId,
            cancellationToken);

        if (accountBalance < totalAmount)
            throw new InvalidOperationException(
                "Insufficient funds.");

        var paymentTransaction = new Transaction(
            userId,
            totalAmount,
            TransactionType.CreditCardInvoicePayment,
            paymentMethod == CreditCardInvoicePaymentMethod.Pix
                ? PaymentMethod.Pix
                : PaymentMethod.Account,
            "Pagamento da fatura do cartão",
            DateTimeOffset.UtcNow,
            request.AccountId);

        paymentTransaction.AssignToCreditCardInvoicePayment(
            invoiceId);

        var payment = new CreditCardInvoicePayment(
            userId,
            invoiceId,
            request.AccountId,
            totalAmount,
            paymentMethod);

        await _transactionRepository.AddAsync(
            paymentTransaction,
            cancellationToken);

        await _paymentRepository.AddAsync(
            payment,
            cancellationToken);

        invoice.MarkAsPaid();

        await _invoiceRepository.SaveChangesAsync(
            cancellationToken);

        return await _invoiceService.GetByIdAsync(
            cardId,
            invoiceId,
            cancellationToken);
    }
}