using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Finance;

namespace What2Gift.Application.Admin.TopUp.RejectTopUpTransaction;

public class RejectTopUpTransactionCommandHandler(IDbContext context) 
    : ICommandHandler<RejectTopUpTransactionCommand, RejectTopUpTransactionResponse>
{
    public async Task<Result<RejectTopUpTransactionResponse>> Handle(
        RejectTopUpTransactionCommand request, 
        CancellationToken cancellationToken)
    {
        var topUpTransaction = await context.TopUpTransactions
            .FirstOrDefaultAsync(t => t.Id == request.TopUpTransactionId, cancellationToken);

        if (topUpTransaction is null)
        {
            return Result.Failure<RejectTopUpTransactionResponse>(
                Error.NotFound("TopUpTransaction.NotFound", "Top-up transaction not found"));
        }

        if (topUpTransaction.Status != TopUpTransactionStatus.Pending)
        {
            return Result.Failure<RejectTopUpTransactionResponse>(
                Error.Validation("TopUpTransaction.AlreadyProcessed", 
                    $"Transaction has already been {topUpTransaction.Status}"));
        }

        // Update transaction status
        topUpTransaction.Status = TopUpTransactionStatus.Rejected;
        topUpTransaction.ApprovedAt = DateTime.UtcNow;
        topUpTransaction.ApprovedBy = request.AdminUserId;
        topUpTransaction.Note = request.Note;

        // Create PaymentTransaction for rejected top-up
        var paymentTransaction = new PaymentTransaction
        {
            Id = Guid.NewGuid(),
            UserId = topUpTransaction.UserId,
            MembershipPlanId = null, // Top-up doesn't have membership plan
            Amount = topUpTransaction.Amount,
            PaymentMethod = "TOP_UP_REJECTED",
            TransactionCode = $"TOPUP_{topUpTransaction.Id}",
            Status = PaymentTransactionStatus.Failed,
            CreatedAt = DateTime.UtcNow,
            PaidAt = null
        };

        context.PaymentTransactions.Add(paymentTransaction);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(new RejectTopUpTransactionResponse
        {
            IsSuccess = true,
            Message = "Top-up transaction rejected"
        });
    }
}

