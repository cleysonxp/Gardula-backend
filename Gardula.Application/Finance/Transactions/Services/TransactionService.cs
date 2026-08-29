using Gardula.Application.Common.DTOs;
using Gardula.Application.Common.Services;
using Gardula.Application.Finance.Accounts.Services;
using Gardula.Application.Finance.Cards.Services;
using Gardula.Application.Finance.Categories.Services;
using Gardula.Application.Finance.Transactions.DTOs;
using Gardula.Domain.Entities.Finance;

namespace Gardula.Application.Finance.Transactions.Services;

public class TransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICardRepository _cardRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICreditCardInvoiceRepository _creditCardInvoiceRepository;
    private readonly CreditCardInvoiceService _creditCardInvoiceService;
    private readonly ICurrentUserService _currentUserService;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository,
        ICardRepository cardRepository,
        ICategoryRepository categoryRepository,
        ICreditCardInvoiceRepository creditCardInvoiceRepository,
        CreditCardInvoiceService creditCardInvoiceService,
        ICurrentUserService currentUserService)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _cardRepository = cardRepository;
        _categoryRepository = categoryRepository;
        _creditCardInvoiceRepository = creditCardInvoiceRepository;
        _creditCardInvoiceService = creditCardInvoiceService;
        _currentUserService = currentUserService;
    }

    public async Task<TransactionResponse> CreateAsync(
        CreateTransactionRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var type = (TransactionType)request.Type;

        if (!Enum.IsDefined(type))
            throw new ArgumentException(
                "Invalid transaction type.",
                nameof(request.Type));

        var paymentMethod = (PaymentMethod)request.PaymentMethod;

        if (!Enum.IsDefined(paymentMethod))
            throw new ArgumentException(
                "Invalid payment method.",
                nameof(request.PaymentMethod));

        Account? account = null;

        if (paymentMethod == PaymentMethod.CreditCard)
        {
            var card = await _cardRepository.GetByIdAsync(
                request.CardId!.Value,
                userId,
                cancellationToken);

            if (card is null || !card.IsActive)
                throw new ArgumentException(
                    "Card not found or inactive.",
                    nameof(request.CardId));
        }
        else
        {
            account = await _accountRepository.GetByIdAsync(
                request.AccountId!.Value,
                userId,
                cancellationToken);

            if (account is null || !account.IsActive)
                throw new ArgumentException(
                    "Account not found or inactive.",
                    nameof(request.AccountId));
        }

        var transaction = new Transaction(
            userId,
            request.Amount,
            type,
            paymentMethod,
            request.Description.Trim(),
            request.Date,
            accountId: request.AccountId,
            cardId: request.CardId,
            categoryId: request.CategoryId,
            installmentGroupId: request.InstallmentGroupId,
            installmentNumber: request.InstallmentNumber,
            totalInstallments: request.TotalInstallments);

        if (paymentMethod != PaymentMethod.CreditCard)
        {
            if (type == TransactionType.Income)
                account!.Credit(request.Amount);

            if (type == TransactionType.Expense)
                account!.Debit(request.Amount);
        }

        await _transactionRepository.AddAsync(
            transaction,
            cancellationToken);

        await _transactionRepository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(transaction);
    }

    public async Task<PagedResponse<TransactionListResponse>> GetAllAsync(
        TransactionFilterRequest filter,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var transactions =
            await _transactionRepository.GetAllByUserIdAsync(
                userId,
                filter,
                cancellationToken);

        var items = transactions.Items
            .Select(MapToListResponse)
            .ToList();

        return new PagedResponse<TransactionListResponse>(
            items,
            transactions.Page,
            transactions.PageSize,
            transactions.TotalItems,
            transactions.TotalPages,
            transactions.HasNextPage);
    }

    public async Task<TransactionResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var transaction =
            await _transactionRepository.GetByIdAsync(
                id,
                userId,
                cancellationToken);

        if (transaction is null)
            return null;

        return MapToResponse(transaction);
    }

    private async Task AssignCreditCardInvoiceAsync(
        Transaction transaction,
        Card card,
        int userId,
        CancellationToken cancellationToken)
    {
        var period = _creditCardInvoiceService.CalculatePeriod(
            transaction.Date,
            card.ClosingDay,
            card.DueDay);

        var invoice = await _creditCardInvoiceRepository.GetByDateAsync(
            userId,
            card.Id,
            transaction.Date,
            cancellationToken);

        if (invoice is null)
        {
            invoice = new CreditCardInvoice(
                userId,
                card.Id,
                period.StartDate,
                period.ClosingDate,
                period.DueDate);

            await _creditCardInvoiceRepository.AddAsync(
                invoice,
                cancellationToken);
        }

        transaction.AssignToCreditCardInvoice(
            invoice.Id);
    }

    private static TransactionResponse MapToResponse(
        Transaction transaction)
    {
        return new TransactionResponse(
            transaction.Id,
            transaction.AccountId,
            transaction.CardId,
            transaction.CategoryId,
            transaction.TransferId,
            transaction.Amount,
            (int)transaction.Type,
            (int)transaction.PaymentMethod,
            transaction.Description,
            transaction.Date,
            transaction.InstallmentGroupId,
            transaction.InstallmentNumber,
            transaction.TotalInstallments,
            transaction.CreatedAt,
            transaction.UpdatedAt);
    }

    private static TransactionListResponse MapToListResponse(
        TransactionListItem item)
    {
        var transaction = item.Transaction;

        return new TransactionListResponse(
            transaction.Id,
            transaction.Description,
            transaction.Amount,
            transaction.Date,
            (int)transaction.Type,
            transaction.Type.ToString(),
            (int)transaction.PaymentMethod,
            transaction.PaymentMethod.ToString(),

            item.CategoryName is not null
                ? new CategorySummaryResponse(
                    transaction.CategoryId!.Value,
                    item.CategoryName)
                : null,

            item.AccountName is not null
                ? new AccountSummaryResponse(
                    transaction.AccountId!.Value,
                    item.AccountName)
                : null,

            item.CardName is not null
                ? new CardSummaryResponse(
                    transaction.CardId!.Value,
                    item.CardName,
                    item.CardLastFourDigits!)
                : null,

            transaction.InstallmentGroupId.HasValue
                ? new InstallmentSummaryResponse(
                    transaction.InstallmentGroupId.Value,
                    transaction.InstallmentNumber!.Value,
                    transaction.TotalInstallments!.Value)
                : null,

            item.Transfer is not null
                ? new TransferSummaryResponse(
                    item.Transfer.Id,
                    item.Transfer.SourceAccountId,
                    item.Transfer.DestinationAccountId)
                : null);
    }

    public async Task<TransactionSummaryResponse> GetSummaryAsync(
        TransactionFilterRequest filter,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        return await _transactionRepository.GetSummaryAsync(
            userId,
            filter,
            cancellationToken);
    }

    public async Task<TransactionDetailResponse?> GetDetailByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        return await _transactionRepository.GetDetailByIdAsync(
            id,
            userId,
            cancellationToken);
    }

    public async Task<TransactionResponse> UpdateAsync(
        int id,
        UpdateTransactionRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var transaction = await _transactionRepository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (transaction is null)
        {
            throw new KeyNotFoundException(
                "Transaction not found.");
        }

        if (transaction.TransferId.HasValue)
        {
            throw new ArgumentException(
                "Transfer transactions must be edited through the transfer flow.");
        }

        if (transaction.InstallmentGroupId.HasValue)
        {
            throw new ArgumentException(
                "Installment transactions must be edited through the installment flow.");
        }

        var category = await _categoryRepository.GetByIdAsync(
            request.CategoryId,
            userId,
            cancellationToken);

        if (category is null)
        {
            throw new ArgumentException(
                "Category not found.",
                nameof(request.CategoryId));
        }

        if (category.Type == CategoryType.Income &&
            transaction.Type != TransactionType.Income)
        {
            throw new ArgumentException(
                "Income category cannot be used for an expense.",
                nameof(request.CategoryId));
        }

        if (category.Type == CategoryType.Expense &&
            transaction.Type != TransactionType.Expense)
        {
            throw new ArgumentException(
                "Expense category cannot be used for an income.",
                nameof(request.CategoryId));
        }

        var paymentMethod = (PaymentMethod)request.PaymentMethod;

        if (!Enum.IsDefined(paymentMethod))
        {
            throw new ArgumentException(
                "Invalid payment method.",
                nameof(request.PaymentMethod));
        }

        if (paymentMethod == PaymentMethod.CreditCard)
        {
            if (!request.CardId.HasValue)
            {
                throw new ArgumentException(
                    "CardId is required for credit card transactions.",
                    nameof(request.CardId));
            }

            if (request.AccountId.HasValue)
            {
                throw new ArgumentException(
                    "AccountId cannot be used with credit card transactions.",
                    nameof(request.AccountId));
            }

            var card = await _cardRepository.GetByIdAsync(
                request.CardId.Value,
                userId,
                cancellationToken);

            if (card is null)
            {
                throw new ArgumentException(
                    "Card not found.",
                    nameof(request.CardId));
            }

            if (!card.IsActive)
            {
                throw new ArgumentException(
                    "Card is inactive.",
                    nameof(request.CardId));
            }
        }
        else
        {
            if (!request.AccountId.HasValue)
            {
                throw new ArgumentException(
                    "AccountId is required for this payment method.",
                    nameof(request.AccountId));
            }

            if (request.CardId.HasValue)
            {
                throw new ArgumentException(
                    "CardId can only be used with credit card transactions.",
                    nameof(request.CardId));
            }

            var account = await _accountRepository.GetByIdAsync(
                request.AccountId.Value,
                userId,
                cancellationToken);

            if (account is null)
            {
                throw new ArgumentException(
                    "Account not found.",
                    nameof(request.AccountId));
            }

            if (!account.IsActive)
            {
                throw new ArgumentException(
                    "Account is inactive.",
                    nameof(request.AccountId));
            }
        }

        transaction.Update(
            request.Amount,
            paymentMethod,
            request.Description,
            request.Date,
            request.AccountId,
            request.CardId,
            request.CategoryId);

        await _transactionRepository.UpdateAsync(
            transaction,
            cancellationToken);

        await _transactionRepository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(transaction);
    }

    public async Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var transaction = await _transactionRepository.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (transaction is null)
        {
            throw new KeyNotFoundException(
                "Transaction not found.");
        }

        if (transaction.TransferId.HasValue)
        {
            throw new ArgumentException(
                "Transfer transactions must be deleted through the transfer flow.");
        }

        if (transaction.InstallmentGroupId.HasValue)
        {
            throw new ArgumentException(
                "Installment transactions must be deleted through the installment flow.");
        }

        await _transactionRepository.DeleteAsync(
            transaction,
            cancellationToken);

        await _transactionRepository.SaveChangesAsync(
            cancellationToken);
    }

    public async Task DeleteInstallmentGroupAsync(
        Guid installmentGroupId,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        await _transactionRepository.DeleteInstallmentGroupAsync(
            installmentGroupId,
            userId,
            cancellationToken);

        await _transactionRepository.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<TransactionResponse> UpdateInstallmentAsync(
        int id,
        UpdateInstallmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var transaction = await _transactionRepository
            .GetInstallmentByIdAsync(
                id,
                userId,
                cancellationToken);

        if (transaction is null)
        {
            throw new KeyNotFoundException(
                "Installment transaction not found.");
        }

        var installmentGroupId = transaction.InstallmentGroupId!.Value;

        var installments = await _transactionRepository
            .GetInstallmentsByGroupIdAsync(
                installmentGroupId,
                userId,
                cancellationToken);

        if (installments.Count == 0)
        {
            throw new KeyNotFoundException(
                "Installment group not found.");
        }

        var category = await _categoryRepository.GetByIdAsync(
            request.CategoryId,
            userId,
            cancellationToken);

        if (category is null)
        {
            throw new ArgumentException(
                "Category not found.",
                nameof(request.CategoryId));
        }

        if (category.Type != CategoryType.Expense)
        {
            throw new ArgumentException(
                "Installment transactions must use an expense category.",
                nameof(request.CategoryId));
        }

        if (request.Amount <= 0)
        {
            throw new ArgumentException(
                "Amount must be greater than zero.",
                nameof(request.Amount));
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            throw new ArgumentException(
                "Description is required.",
                nameof(request.Description));
        }

        var totalInstallments = installments.Count;

        var installmentAmount = Math.Round(
            request.Amount / totalInstallments,
            2,
            MidpointRounding.AwayFromZero);

        foreach (var installment in installments)
        {
            var installmentNumber = installment.InstallmentNumber!.Value;

            var amount = installmentNumber == totalInstallments
                ? request.Amount -
                  (installmentAmount * (totalInstallments - 1))
                : installmentAmount;

            var date = request.Date.AddMonths(
                installmentNumber - 1);

            installment.Update(
                amount,
                installment.PaymentMethod,
                request.Description.Trim(),
                date,
                installment.AccountId,
                installment.CardId,
                request.CategoryId);
        }

        await _transactionRepository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(
            installments.First(x => x.Id == id));
    }

}