using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Admin.Memberships.GetAllMemberships;

public class GetAllMembershipsQueryHandler(IDbContext context) : IQueryHandler<GetAllMembershipsQuery, Page<AdminMembershipResponse>>
{
    public async Task<Result<Page<AdminMembershipResponse>>> Handle(GetAllMembershipsQuery request, CancellationToken cancellationToken)
    {
        var query = context.Memberships
            .Include(m => m.User)
            .Include(m => m.MembershipPlan)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(m => 
                m.User.Username.Contains(request.SearchTerm) || 
                m.User.Email.Contains(request.SearchTerm) ||
                m.MembershipPlan.Name.Contains(request.SearchTerm));
        }

        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
            {
                query = query.Where(m => m.EndDate > DateOnly.FromDateTime(DateTime.Now));
            }
            else
            {
                query = query.Where(m => m.EndDate <= DateOnly.FromDateTime(DateTime.Now));
            }
        }

        if (request.MembershipPlanId.HasValue)
        {
            query = query.Where(m => m.MembershipPlanId == request.MembershipPlanId.Value);
        }

        if (request.StartDateFrom.HasValue)
        {
            query = query.Where(m => m.StartDate >= DateOnly.FromDateTime(request.StartDateFrom.Value));
        }

        if (request.StartDateTo.HasValue)
        {
            query = query.Where(m => m.StartDate <= DateOnly.FromDateTime(request.StartDateTo.Value));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var result = await query
            .ApplyPagination(request.PageNumber, request.PageSize)
            .Select(m => new AdminMembershipResponse
            {
                Id = m.Id,
                UserId = m.UserId,
                UserName = m.User.Username,
                UserEmail = m.User.Email,
                MembershipPlanId = m.MembershipPlanId,
                MembershipPlanName = m.MembershipPlan.Name,
                MembershipPlanPrice = m.MembershipPlan.Price,
                StartDate = m.StartDate,
                EndDate = m.EndDate,
                IsActive = m.EndDate > DateOnly.FromDateTime(DateTime.Now),
                DaysRemaining = m.EndDate > DateOnly.FromDateTime(DateTime.Now) 
                    ? (m.EndDate.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days 
                    : 0
            })
            .ToListAsync(cancellationToken);

        return new Page<AdminMembershipResponse>(
            result,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}
