using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Users.Helpers;
using What2Gift.Domain.Common;
using What2Gift.Domain.Finance;
using What2Gift.Domain.Users;

namespace What2Gift.Application.TopUp.CreateTopUpTransaction;

public class CreateTopUpTransactionCommandHandler(IDbContext context) 
    : ICommandHandler<CreateTopUpTransactionCommand, CreateTopUpTransactionResponse>
{
    public async Task<Result<CreateTopUpTransactionResponse>> Handle(
        CreateTopUpTransactionCommand request, 
        CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<CreateTopUpTransactionResponse>(
                Error.NotFound("User.NotFound", "User not found"));
        }

        const decimal minimumAmount = 1000; // Minimum 1000 VND = 1000 points

        if (request.Amount < minimumAmount)
        {
            return Result.Failure<CreateTopUpTransactionResponse>(
                Error.Validation("TopUp.InvalidAmount", $"Minimum top-up amount is {minimumAmount} VND (1000 points)"));
        }

        // Calculate points (1000 points = 1000 VND, so 1:1 ratio)
        var points = (int)request.Amount;

        // Generate TopUpCode if user doesn't have one yet (for existing users)
        if (!user.TopUpCode.HasValue)
        {
            user.TopUpCode = await TopUpCodeGenerator.GenerateUniqueTopUpCodeAsync(context, cancellationToken);
        }
        
        var transferContent = $"nap{user.TopUpCode.Value:D4}"; // Format as nap0000, nap0001, etc.

        // Create top-up transaction
        var topUpTransaction = new TopUpTransaction
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Amount = request.Amount,
            Points = points,
            TransferContent = transferContent,
            Status = TopUpTransactionStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        context.TopUpTransactions.Add(topUpTransaction);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(new CreateTopUpTransactionResponse
        {
            TopUpTransactionId = topUpTransaction.Id,
            TransferContent = transferContent,
            Amount = request.Amount,
            Points = points
        });
    }
}

