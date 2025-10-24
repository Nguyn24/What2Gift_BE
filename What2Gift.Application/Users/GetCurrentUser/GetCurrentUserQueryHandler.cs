using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Authentication;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Users.Errors;

namespace What2Gift.Application.Users.GetCurrentUser
{
    public class GetCurrentUserQueryHandler(IDbContext context, IUserContext userContext) : IQueryHandler<GetCurrentUserQuery, GetCurrentUserResponse>
    {
        public async Task<Result<GetCurrentUserResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = userContext.UserId;

            var user = await context.Users
                .Include(u => u.Membership)
                .FirstOrDefaultAsync(p => p.Id == currentUserId, cancellationToken);

            if (user == null)
                return Result.Failure<GetCurrentUserResponse>(UserErrors.NotFound(currentUserId));

            var response = new GetCurrentUserResponse
            {
                FullName = user.Username,
                Email = user.Email,
                Role = user.Role.ToString(),
                Status = user.Status,
                MembershipStatus = user.MembershipStatus,
                AvatarUrl = user.AvatarUrl,
                Membership = user.Membership == null ? null : new Membership
                {
                    StartDate = user.Membership.StartDate,
                    EndDate = user.Membership.EndDate
                }
            };

            return response;
        }

    }
}
