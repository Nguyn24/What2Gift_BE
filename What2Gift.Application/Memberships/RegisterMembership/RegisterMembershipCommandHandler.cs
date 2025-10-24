using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Finance;
using What2Gift.Domain.Users;
using What2Gift.Application.Abstraction.Services;

namespace What2Gift.Application.Memberships.RegisterMembership;

public class RegisterMembershipCommandHandler(
    IDbContext context,
    IVnPayService vnPayService) : ICommandHandler<RegisterMembershipCommand, RegisterMembershipResponse>
{
    public async Task<Result<RegisterMembershipResponse>> Handle(RegisterMembershipCommand request, CancellationToken cancellationToken)
    {
        // Check if user exists
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<RegisterMembershipResponse>(Error.NotFound("User.NotFound", "User not found"));
        }

        // Check if membership plan exists
        var membershipPlan = await context.MembershipPlans
            .FirstOrDefaultAsync(mp => mp.Id == request.MembershipPlanId, cancellationToken);

        if (membershipPlan is null)
        {
            return Result.Failure<RegisterMembershipResponse>(Error.NotFound("MembershipPlan.NotFound", "Membership plan not found"));
        }

        // Check if user already has an active membership
        var existingMembership = await context.Memberships
            .Where(m => m.UserId == request.UserId && m.EndDate > DateOnly.FromDateTime(DateTime.Now))
            .FirstOrDefaultAsync(cancellationToken);

        if (existingMembership is not null)
        {
            return Result.Failure<RegisterMembershipResponse>(Error.Validation("Membership.AlreadyActive", "User already has an active membership"));
        }

        // Create payment transaction
        var paymentTransaction = new PaymentTransaction
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            MembershipPlanId = request.MembershipPlanId,
            Amount = membershipPlan.Price,
            PaymentMethod = "VNPAY",
            TransactionCode = $"MEM_{request.UserId}_{request.MembershipPlanId}_{DateTime.Now:yyyyMMddHHmmss}",
            Status = PaymentTransactionStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        context.PaymentTransactions.Add(paymentTransaction);
        await context.SaveChangesAsync(cancellationToken);

        // Create VNPAY payment URL
        var paymentUrl = vnPayService.CreatePaymentUrl(
            request.UserId,
            request.MembershipPlanId,
            membershipPlan.Price,
            request.ReturnUrl);

        return Result.Success(new RegisterMembershipResponse
        {
            PaymentUrl = paymentUrl,
            PaymentTransactionId = paymentTransaction.Id
        });
    }
}
