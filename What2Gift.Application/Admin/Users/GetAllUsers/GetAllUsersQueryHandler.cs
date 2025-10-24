using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Admin.Users.GetAllUsers;

public class GetAllUsersQueryHandler(IDbContext context) : IQueryHandler<GetAllUsersQuery, Page<AdminUserResponse>>
{
    public async Task<Result<Page<AdminUserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var query = context.Users
            .Include(u => u.Membership)
            .ThenInclude(m => m.MembershipPlan)
            .Include(u => u.PaymentTransactions)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(u => 
                u.Username.Contains(request.SearchTerm) || 
                u.Email.Contains(request.SearchTerm));
        }

        if (request.Status.HasValue)
        {
            query = query.Where(u => u.Status == request.Status.Value);
        }

        if (request.CreatedFrom.HasValue)
        {
            query = query.Where(u => u.CreatedAt >= request.CreatedFrom.Value);
        }

        if (request.CreatedTo.HasValue)
        {
            query = query.Where(u => u.CreatedAt <= request.CreatedTo.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var result = await query
            .ApplyPagination(request.PageNumber, request.PageSize)
            .Select(u => new AdminUserResponse
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Status = u.Status,
                CreatedAt = u.CreatedAt,
                IsVerified = u.IsVerified,
                HasActiveMembership = u.Membership != null && u.Membership.EndDate > DateOnly.FromDateTime(DateTime.Now),
                MembershipPlanName = u.Membership != null ? u.Membership.MembershipPlan.Name : null,
                MembershipExpiresAt = u.Membership != null ? u.Membership.EndDate.ToDateTime(TimeOnly.MinValue) : null,
                TotalOrders = u.PaymentTransactions.Count(pt => pt.Status == Domain.Finance.PaymentTransactionStatus.Success),
                TotalSpent = u.PaymentTransactions
                    .Where(pt => pt.Status == Domain.Finance.PaymentTransactionStatus.Success)
                    .Sum(pt => pt.Amount)
            })
            .ToListAsync(cancellationToken);

        return new Page<AdminUserResponse>(
            result,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}
