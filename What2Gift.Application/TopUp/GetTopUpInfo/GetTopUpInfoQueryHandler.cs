using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Application.Users.Helpers;
using What2Gift.Domain.Common;
using What2Gift.Domain.Finance;
using What2Gift.Domain.Users;

namespace What2Gift.Application.TopUp.GetTopUpInfo;

public class GetTopUpInfoQueryHandler(IDbContext context) : IQueryHandler<GetTopUpInfoQuery, GetTopUpInfoResponse>
{
    public async Task<Result<GetTopUpInfoResponse>> Handle(GetTopUpInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<GetTopUpInfoResponse>(Error.NotFound("User.NotFound", "User not found"));
        }

        // Get active bank account from database
        var bankAccount = await context.BankAccounts
            .FirstOrDefaultAsync(b => b.IsActive, cancellationToken);

        if (bankAccount is null)
        {
            return Result.Failure<GetTopUpInfoResponse>(
                Error.NotFound("BankAccount.NotFound", "No active bank account found"));
        }

        // Generate TopUpCode if user doesn't have one yet (for existing users)
        if (!user.TopUpCode.HasValue)
        {
            user.TopUpCode = await TopUpCodeGenerator.GenerateUniqueTopUpCodeAsync(context, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        
        var transferContent = $"nap{user.TopUpCode.Value:D4}"; // Format as nap0000, nap0001, etc.

        return Result.Success(new GetTopUpInfoResponse
        {
            TransferContent = transferContent,
            BankAccountNumber = bankAccount.AccountNumber,
            BankName = bankAccount.BankName,
            AccountHolderName = bankAccount.AccountHolderName,
            QrCodeUrl = bankAccount.QrCodeUrl,
            CurrentPoints = user.W2GPoints
        });
    }
}

