using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Finance;

namespace What2Gift.Application.Admin.TopUp.ApproveTopUpTransaction;

public class ApproveTopUpTransactionCommandHandler(IDbContext context) 
    : ICommandHandler<ApproveTopUpTransactionCommand, ApproveTopUpTransactionResponse>
{
    public async Task<Result<ApproveTopUpTransactionResponse>> Handle(
        ApproveTopUpTransactionCommand request, 
        CancellationToken cancellationToken)
    {
        var topUpTransaction = await context.TopUpTransactions
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == request.TopUpTransactionId, cancellationToken);

        if (topUpTransaction is null)
        {
            return Result.Failure<ApproveTopUpTransactionResponse>(
                Error.NotFound("TopUpTransaction.NotFound", "Top-up transaction not found"));
        }

        if (topUpTransaction.Status != TopUpTransactionStatus.Pending)
        {
            return Result.Failure<ApproveTopUpTransactionResponse>(
                Error.Validation("TopUpTransaction.AlreadyProcessed", 
                    $"Transaction has already been {topUpTransaction.Status}"));
        }

        // Update transaction status
        topUpTransaction.Status = TopUpTransactionStatus.Approved;
        topUpTransaction.ApprovedAt = DateTime.UtcNow;
        topUpTransaction.ApprovedBy = request.AdminUserId;
        topUpTransaction.Note = request.Note;

        // Add points to user account
        topUpTransaction.User.W2GPoints += topUpTransaction.Points;

        // Create PaymentTransaction for approved top-up
        var paymentTransaction = new PaymentTransaction
        {
            Id = Guid.NewGuid(),
            UserId = topUpTransaction.UserId,
            MembershipPlanId = null, // Top-up doesn't have membership plan
            Amount = topUpTransaction.Amount,
            PaymentMethod = "TOP_UP_APPROVED",
            TransactionCode = $"TOPUP_{topUpTransaction.Id}",
            Status = PaymentTransactionStatus.Success,
            CreatedAt = DateTime.UtcNow,
            PaidAt = DateTime.UtcNow
        };

        context.PaymentTransactions.Add(paymentTransaction);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(new ApproveTopUpTransactionResponse
        {
            IsSuccess = true,
            Message = "Top-up transaction approved and points added successfully",
            PointsAdded = topUpTransaction.Points,
            NewBalance = topUpTransaction.User.W2GPoints
        });
    }
}

