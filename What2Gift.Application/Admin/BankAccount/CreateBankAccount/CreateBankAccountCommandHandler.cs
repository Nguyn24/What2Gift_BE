using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Finance;

namespace What2Gift.Application.Admin.BankAccount.CreateBankAccount;

public class CreateBankAccountCommandHandler(IDbContext context) 
    : ICommandHandler<CreateBankAccountCommand, CreateBankAccountResponse>
{
    public async Task<Result<CreateBankAccountResponse>> Handle(
        CreateBankAccountCommand request, 
        CancellationToken cancellationToken)
    {
        // Check if there's already an active bank account
        var existingActiveAccount = await context.BankAccounts
            .FirstOrDefaultAsync(b => b.IsActive, cancellationToken);

        if (existingActiveAccount is not null)
        {
            return Result.Failure<CreateBankAccountResponse>(
                Error.Validation("BankAccount.AlreadyExists", 
                    "An active bank account already exists. Please deactivate it first or update the existing one."));
        }

        // Create new bank account
        var bankAccount = new Domain.Finance.BankAccount
        {
            Id = Guid.NewGuid(),
            BankName = request.BankName,
            AccountNumber = request.AccountNumber,
            AccountHolderName = request.AccountHolderName,
            QrCodeUrl = request.QrCodeUrl,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        context.BankAccounts.Add(bankAccount);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(new CreateBankAccountResponse
        {
            BankAccountId = bankAccount.Id,
            BankName = bankAccount.BankName,
            AccountNumber = bankAccount.AccountNumber,
            AccountHolderName = bankAccount.AccountHolderName,
            QrCodeUrl = bankAccount.QrCodeUrl
        });
    }
}

