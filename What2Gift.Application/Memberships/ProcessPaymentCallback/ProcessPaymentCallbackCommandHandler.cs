using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Finance;
using What2Gift.Domain.Users;

namespace What2Gift.Application.Memberships.ProcessPaymentCallback;

public class ProcessPaymentCallbackCommandHandler(IDbContext context) : ICommandHandler<ProcessPaymentCallbackCommand, ProcessPaymentCallbackResponse>
{
    public async Task<Result<ProcessPaymentCallbackResponse>> Handle(ProcessPaymentCallbackCommand request, CancellationToken cancellationToken)
    {
        // Parse transaction ID to get user and membership plan info
        var transactionParts = request.TransactionId.Split('_');
        if (transactionParts.Length < 3)
        {
            return Result.Failure<ProcessPaymentCallbackResponse>(Error.Validation("Payment.InvalidTransactionId", "Invalid transaction ID format"));
        }

        if (!Guid.TryParse(transactionParts[1], out var userId) || 
            !Guid.TryParse(transactionParts[2], out var membershipPlanId))
        {
            return Result.Failure<ProcessPaymentCallbackResponse>(Error.Validation("Payment.InvalidTransactionId", "Invalid user or membership plan ID"));
        }

        // Find the payment transaction
        var paymentTransaction = await context.PaymentTransactions
            .Include(pt => pt.User)
            .Include(pt => pt.MembershipPlan)
            .FirstOrDefaultAsync(pt => pt.TransactionCode == request.TransactionId, cancellationToken);

        if (paymentTransaction is null)
        {
            return Result.Failure<ProcessPaymentCallbackResponse>(Error.NotFound("Payment.TransactionNotFound", "Payment transaction not found"));
        }

        // Update payment transaction status
        paymentTransaction.Status = request.IsSuccess ? PaymentTransactionStatus.Success : PaymentTransactionStatus.Failed;
        paymentTransaction.PaidAt = request.IsSuccess ? DateTime.UtcNow : null;

        if (request.IsSuccess)
        {
            // Create membership
            var membership = new Membership
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                MembershipPlanId = membershipPlanId,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)) // Default 1 month, can be configured based on plan
            };

            context.Memberships.Add(membership);

            // Create revenue record for finance tracking
            var revenue = new Revenue
            {
                Id = Guid.NewGuid(),
                IncomeSourceId = await GetOrCreateMembershipIncomeSource(cancellationToken),
                Amount = request.Amount,
                Description = $"Membership payment for {paymentTransaction.MembershipPlan?.Name}",
                ReceivedAt = DateTime.UtcNow
            };

            context.Revenues.Add(revenue);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(new ProcessPaymentCallbackResponse
            {
                IsSuccess = true,
                Message = "Payment processed successfully and membership activated",
                MembershipId = membership.Id
            });
        }
        else
        {
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(new ProcessPaymentCallbackResponse
            {
                IsSuccess = false,
                Message = "Payment failed"
            });
        }
    }

    private async Task<Guid> GetOrCreateMembershipIncomeSource(CancellationToken cancellationToken)
    {
        var incomeSource = await context.IncomeSources
            .FirstOrDefaultAsync(incomeSource => incomeSource.Name == "Membership Subscriptions", cancellationToken);

        if (incomeSource is null)
        {
            incomeSource = new IncomeSource
            {
                Id = Guid.NewGuid(),
                Name = "Membership Subscriptions",
                Description = "Revenue from membership plan subscriptions"
            };

            context.IncomeSources.Add(incomeSource);
            await context.SaveChangesAsync(cancellationToken);
        }

        return incomeSource.Id;
    }
}
