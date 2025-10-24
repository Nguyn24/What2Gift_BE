using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;
using What2Gift.Domain.Users;
using Membership = What2Gift.Application.Users.GetAllUser.Membership;

namespace What2Gift.Application.Users.GetAllUser;

public sealed class GetUsersQueryHandler(IDbContext context)
    : IQueryHandler<GetUsersQuery, Page<GetUsersResponse>>
{
    public async Task<Result<Page<GetUsersResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = context.Users
            .Where(u => u.Role != UserRole.Admin);

        var totalCount = await query.CountAsync(cancellationToken);

        var result = await query
            .Include(q => q.Membership)
            .ApplyPagination(request.PageNumber, request.PageSize)
            .Select(u => new GetUsersResponse
            {
                Id = u.Id,
                Name = u.Username,
                Email = u.Email,
                Role = u.Role,
                Status = u.Status,
                MembershipStatus = u.MembershipStatus,
                AvatarUrl = u.AvatarUrl,
                Membership = u.Membership == null ? null : new Membership
                {
                    StartDate = u.Membership.StartDate,
                    EndDate = u.Membership.EndDate
                }
            })
            .ToListAsync(cancellationToken);

        return new Page<GetUsersResponse>(
            result,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}