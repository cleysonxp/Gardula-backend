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

        if (request.Amount <= 0)
            throw new ArgumentException(
                "Amount must be greater than zero.",
                nameof(request.Amount));

        if (request.TotalInstallments.HasValue &&
            request.TotalInstallments.Value <= 1)
        {
            throw new ArgumentException(
                "Total installments must be greater than one.",
                nameof(request.TotalInstallments));
        }

        if (request.TotalInstallments.HasValue &&
            paymentMethod != PaymentMethod.CreditCard)
        {
            throw new ArgumentException(
                "Installments are only available for credit card transactions.",
                nameof(request.TotalInstallments));
        }

        if (request.TotalInstallments.HasValue &&
            type != TransactionType.Expense)
        {
            throw new ArgumentException(
                "Installments are only available for expense transactions.",
                nameof(request.Type));
        }

        Account? account = null;
        Card? card = null;

        if (paymentMethod == PaymentMethod.CreditCard)
        {
            if (!request.CardId.HasValue)
                throw new ArgumentException(
                    "CardId is required for credit card transactions.",
                    nameof(request.CardId));

            if (request.AccountId.HasValue)
                throw new ArgumentException(
                    "AccountId cannot be used with credit card transactions.",
                    nameof(request.AccountId));

            card = await _cardRepository.GetByIdAsync(
                request.CardId.Value,
                userId,
                cancellationToken);

            if (card is null || !card.IsActive)
                throw new ArgumentException(
                    "Card not found or inactive.",
                    nameof(request.CardId));
        }
        else
        {
            if (!request.AccountId.HasValue)
                throw new ArgumentException(
                    "AccountId is required for this payment method.",
                    nameof(request.AccountId));

            if (request.CardId.HasValue)
                throw new ArgumentException(
                    "CardId can only be used with credit card transactions.",
                    nameof(request.CardId));

            account = await _accountRepository.GetByIdAsync(
                request.AccountId.Value,
                userId,
                cancellationToken);

            if (account is null || !account.IsActive)
                throw new ArgumentException(
                    "Account not found or inactive.",
                    nameof(request.AccountId));
        }

        if (request.TotalInstallments.HasValue)
        {
            var totalInstallments = request.TotalInstallments.Value;
            var installmentGroupId = Guid.NewGuid();

            var installmentAmount = Math.Round(
                request.Amount / totalInstallments,
                2,
                MidpointRounding.AwayFromZero);

            var transactions = new List<Transaction>();

            for (var installmentNumber = 1;
                 installmentNumber <= totalInstallments;
                 installmentNumber++)
            {
                var amount = installmentNumber == totalInstallments
                    ? request.Amount -
                      (installmentAmount * (totalInstallments - 1))
                    : installmentAmount;

                var date = request.Date.AddMonths(
                    installmentNumber - 1);

                var transaction = new Transaction(
                    userId,
                    amount,
                    type,
                    paymentMethod,
                    request.Description.Trim(),
                    date,
                    accountId: null,
                    cardId: request.CardId,
                    categoryId: request.CategoryId,
                    installmentGroupId: installmentGroupId,
                    installmentNumber: installmentNumber,
                    totalInstallments: totalInstallments);

                transactions.Add(transaction);

                await _transactionRepository.AddAsync(
                    transaction,
                    cancellationToken);
            }

            await _transactionRepository.SaveChangesAsync(
                cancellationToken);

            foreach (var transaction in transactions)
            {
                await AssignCreditCardInvoiceAsync(
                    transaction,
                    card!,
                    userId,
                    cancellationToken);
            }

            await _transactionRepository.SaveChangesAsync(
                cancellationToken);

            return MapToResponse(transactions[0]);
        }

        var transactionSingle = new Transaction(
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
            transactionSingle,
            cancellationToken);

        await _transactionRepository.SaveChangesAsync(
            cancellationToken);

        if (paymentMethod == PaymentMethod.CreditCard)
        {
            await AssignCreditCardInvoiceAsync(
                transactionSingle,
                card!,
                userId,
                cancellationToken);

            await _transactionRepository.SaveChangesAsync(
                cancellationToken);
        }

        return MapToResponse(transactionSingle);
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

        invoice.AddAmount(transaction.Amount);

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

        var newType = (TransactionType)request.Type;

        if (!Enum.IsDefined(newType))
        {
            throw new ArgumentException(
                "Invalid transaction type.",
                nameof(request.Type));
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
            newType != TransactionType.Income)
        {
            throw new ArgumentException(
                "Income category cannot be used for an expense.",
                nameof(request.CategoryId));
        }

        if (category.Type == CategoryType.Expense &&
            newType != TransactionType.Expense)
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

        Account? newAccount = null;
        Card? newCard = null;

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

            newCard = await _cardRepository.GetByIdAsync(
                request.CardId.Value,
                userId,
                cancellationToken);

            if (newCard is null)
            {
                throw new ArgumentException(
                    "Card not found.",
                    nameof(request.CardId));
            }

            if (!newCard.IsActive)
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

            newAccount = await _accountRepository.GetByIdAsync(
                request.AccountId.Value,
                userId,
                cancellationToken);

            if (newAccount is null)
            {
                throw new ArgumentException(
                    "Account not found.",
                    nameof(request.AccountId));
            }

            if (!newAccount.IsActive)
            {
                throw new ArgumentException(
                    "Account is inactive.",
                    nameof(request.AccountId));
            }
        }

        var oldPaymentMethod = transaction.PaymentMethod;
        var oldType = transaction.Type;
        var oldAmount = transaction.Amount;
        var oldAccountId = transaction.AccountId;
        var oldCardId = transaction.CardId;
        var oldInvoiceId = transaction.CreditCardInvoiceId;

        if (oldPaymentMethod != PaymentMethod.CreditCard)
        {
            if (!oldAccountId.HasValue)
            {
                throw new InvalidOperationException(
                    "Transaction account is missing.");
            }

            var oldAccount = await _accountRepository.GetByIdAsync(
                oldAccountId.Value,
                userId,
                cancellationToken);

            if (oldAccount is null)
            {
                throw new ArgumentException(
                    "Account not found.",
                    nameof(transaction.AccountId));
            }

            if (oldType == TransactionType.Income)
            {
                oldAccount.Debit(oldAmount);
            }
            else if (oldType == TransactionType.Expense)
            {
                oldAccount.Credit(oldAmount);
            }
        }
        else
        {
            if (oldCardId.HasValue &&
                oldInvoiceId.HasValue)
            {
                var oldInvoice = await _creditCardInvoiceRepository.GetByIdAsync(
                    userId,
                    oldCardId.Value,
                    oldInvoiceId.Value,
                    cancellationToken);

                if (oldInvoice is not null)
                {
                    oldInvoice.RemoveAmount(oldAmount);
                }
            }
        }

        transaction.Update(
            request.Amount,
            newType,
            paymentMethod,
            request.Description.Trim(),
            request.Date,
            request.AccountId,
            request.CardId,
            request.CategoryId);

        if (paymentMethod != PaymentMethod.CreditCard)
        {
            if (newType == TransactionType.Income)
            {
                newAccount!.Credit(request.Amount);
            }
            else if (newType == TransactionType.Expense)
            {
                newAccount!.Debit(request.Amount);
            }
        }
        else
        {
            await AssignCreditCardInvoiceAsync(
                transaction,
                newCard!,
                userId,
                cancellationToken);
        }

        await _transactionRepository.UpdateAsync(
            transaction,
            cancellationToken);

        await _transactionRepository.SaveChangesAsync(
            cancellationToken);

        await _creditCardInvoiceRepository.SaveChangesAsync(
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

        if (transaction.PaymentMethod != PaymentMethod.CreditCard)
        {
            var account = await _accountRepository.GetByIdAsync(
                transaction.AccountId!.Value,
                userId,
                cancellationToken);

            if (account is null)
            {
                throw new ArgumentException(
                    "Account not found.",
                    nameof(transaction.AccountId));
            }

            if (transaction.Type == TransactionType.Income)
            {
                account.Debit(transaction.Amount);
            }

            if (transaction.Type == TransactionType.Expense)
            {
                account.Credit(transaction.Amount);
            }
        }
        else
        {
            if (!transaction.CardId.HasValue ||
                !transaction.CreditCardInvoiceId.HasValue)
            {
                throw new InvalidOperationException(
                    "Credit card transaction is not linked to an invoice.");
            }

            var invoice = await _creditCardInvoiceRepository.GetByIdAsync(
                userId,
                transaction.CardId.Value,
                transaction.CreditCardInvoiceId.Value,
                cancellationToken);

            if (invoice is null)
            {
                throw new ArgumentException(
                    "Credit card invoice not found.",
                    nameof(transaction.CreditCardInvoiceId));
            }

            invoice.RemoveAmount(transaction.Amount);
        }

        await _transactionRepository.DeleteAsync(
            transaction,
            cancellationToken);

        await _transactionRepository.SaveChangesAsync(
            cancellationToken);

        if (transaction.PaymentMethod == PaymentMethod.CreditCard)
        {
            await _creditCardInvoiceRepository.SaveChangesAsync(
                cancellationToken);
        }
    }

    public async Task DeleteInstallmentGroupAsync(
        Guid installmentGroupId,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        var installments =
            await _transactionRepository.GetInstallmentsByGroupIdAsync(
                installmentGroupId,
                userId,
                cancellationToken);

        if (installments.Count == 0)
        {
            throw new KeyNotFoundException(
                "Installment group not found.");
        }

        foreach (var installment in installments)
        {
            if (!installment.CreditCardInvoiceId.HasValue)
                continue;

            if (!installment.CardId.HasValue)
                continue;

            var invoice = await _creditCardInvoiceRepository.GetByIdAsync(
                userId,
                installment.CardId.Value,
                installment.CreditCardInvoiceId.Value,
                cancellationToken);

            if (invoice is null)
                continue;

            invoice.RemoveAmount(installment.Amount);
        }

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

        var transaction = await _transactionRepository.GetInstallmentByIdAsync(
            id,
            userId,
            cancellationToken);

        if (transaction is null)
            throw new KeyNotFoundException(
                "Installment transaction not found.");

        if (!transaction.InstallmentGroupId.HasValue ||
            !transaction.TotalInstallments.HasValue ||
            !transaction.InstallmentNumber.HasValue)
        {
            throw new ArgumentException(
                "Transaction is not an installment.");
        }

        if (request.Amount <= 0)
        {
            throw new ArgumentException(
                "Amount must be greater than zero.",
                nameof(request.Amount));
        }

        if (request.TotalInstallments <= 0)
        {
            throw new ArgumentException(
                "TotalInstallments must be greater than zero.",
                nameof(request.TotalInstallments));
        }

        var currentTotalInstallments =
            transaction.TotalInstallments.Value;

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

        if (!transaction.CardId.HasValue)
        {
            throw new InvalidOperationException(
                "Installment transaction must have a credit card.");
        }

        var card = await _cardRepository.GetByIdAsync(
            transaction.CardId.Value,
            userId,
            cancellationToken);

        if (card is null || !card.IsActive)
        {
            throw new ArgumentException(
                "Card not found or inactive.",
                nameof(transaction.CardId));
        }

        var installments =
            await _transactionRepository.GetInstallmentsByGroupIdAsync(
                transaction.InstallmentGroupId.Value,
                userId,
                cancellationToken);

        if (installments.Count != currentTotalInstallments)
        {
            throw new InvalidOperationException(
                "Installment group is incomplete.");
        }

        /*
         * Remove the current amounts from their invoices.
         */
        foreach (var installment in installments)
        {
            if (!installment.CreditCardInvoiceId.HasValue)
                continue;

            var invoice = await _creditCardInvoiceRepository.GetByIdAsync(
                userId,
                transaction.CardId.Value,
                installment.CreditCardInvoiceId.Value,
                cancellationToken);

            if (invoice is null)
                continue;

            invoice.RemoveAmount(installment.Amount);
        }

        /*
         * Update existing installments that will remain.
         */
        var installmentAmount = Math.Round(
            request.Amount / request.TotalInstallments,
            2,
            MidpointRounding.AwayFromZero);

        var installmentsToKeep = installments
            .Where(installment =>
                installment.InstallmentNumber!.Value <=
                request.TotalInstallments)
            .ToList();

        foreach (var installment in installmentsToKeep)
        {
            var installmentNumber =
                installment.InstallmentNumber!.Value;

            var amount = installmentNumber == request.TotalInstallments
                ? request.Amount -
                  (installmentAmount * (request.TotalInstallments - 1))
                : installmentAmount;

            var date = request.Date.AddMonths(
                installmentNumber - 1);

            installment.UpdateInstallment(
                amount,
                request.Description.Trim(),
                date,
                request.CategoryId,
                request.TotalInstallments);

            await AssignCreditCardInvoiceAsync(
                installment,
                card,
                userId,
                cancellationToken);
        }

        /*
         * Create installments when increasing the quantity.
         */
        if (request.TotalInstallments > currentTotalInstallments)
        {
            for (
                var installmentNumber = currentTotalInstallments + 1;
                installmentNumber <= request.TotalInstallments;
                installmentNumber++)
            {
                var amount = installmentNumber == request.TotalInstallments
                    ? request.Amount -
                      (installmentAmount *
                       (request.TotalInstallments - 1))
                    : installmentAmount;

                var date = request.Date.AddMonths(
                    installmentNumber - 1);

                var installment = new Transaction(
                    userId,
                    amount,
                    TransactionType.Expense,
                    PaymentMethod.CreditCard,
                    request.Description.Trim(),
                    date,
                    accountId: null,
                    cardId: transaction.CardId,
                    categoryId: request.CategoryId,
                    installmentGroupId: transaction.InstallmentGroupId,
                    installmentNumber: installmentNumber,
                    totalInstallments: request.TotalInstallments);

                await _transactionRepository.AddAsync(
                    installment,
                    cancellationToken);

                await AssignCreditCardInvoiceAsync(
                    installment,
                    card,
                    userId,
                    cancellationToken);
            }
        }

        /*
         * Remove installments when decreasing the quantity.
         */
        if (request.TotalInstallments < currentTotalInstallments)
        {
            var installmentsToDelete = installments
                .Where(installment =>
                    installment.InstallmentNumber!.Value >
                    request.TotalInstallments)
                .ToList();

            foreach (var installment in installmentsToDelete)
            {
                await _transactionRepository.DeleteAsync(
                    installment,
                    cancellationToken);
            }
        }

        await _transactionRepository.SaveChangesAsync(
            cancellationToken);

        var updatedTransaction =
            await _transactionRepository.GetInstallmentByIdAsync(
                id,
                userId,
                cancellationToken);

        return MapToResponse(
            updatedTransaction!);
    }
}