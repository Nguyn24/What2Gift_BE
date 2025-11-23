using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Finance;
using What2Gift.Domain.Users;

namespace What2Gift.Application.Memberships.RegisterMembership;

public class RegisterMembershipCommandHandler(
    IDbContext context) : ICommandHandler<RegisterMembershipCommand, RegisterMembershipResponse>
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

        // Calculate required points (1000 points = 1000 VND)
        var requiredPoints = (int)membershipPlan.Price;

        // Check if user has enough points
        if (user.W2GPoints < requiredPoints)
        {
            return Result.Failure<RegisterMembershipResponse>(
                Error.Validation("Membership.InsufficientPoints", 
                    $"Insufficient points. Required: {requiredPoints}, Available: {user.W2GPoints}"));
        }

        // Deduct points from user account
        user.W2GPoints -= requiredPoints;

        // Create membership
        var membership = new Membership
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            MembershipPlanId = request.MembershipPlanId,
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)) // Default 1 month, can be configured based on plan
        };

        context.Memberships.Add(membership);

        // Create PaymentTransaction for membership registration (for admin payment tracking)
        // But not returned in response as requested
        var paymentTransaction = new PaymentTransaction
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            MembershipPlanId = request.MembershipPlanId,
            Amount = membershipPlan.Price,
            PaymentMethod = "W2G_POINTS",
            TransactionCode = $"MEM_{request.UserId}_{request.MembershipPlanId}_{DateTime.Now:yyyyMMddHHmmss}",
            Status = PaymentTransactionStatus.Success,
            CreatedAt = DateTime.UtcNow,
            PaidAt = DateTime.UtcNow
        };

        context.PaymentTransactions.Add(paymentTransaction);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(new RegisterMembershipResponse
        {
            MembershipId = membership.Id,
            PointsUsed = requiredPoints,
            RemainingPoints = user.W2GPoints
        });
    }
}
