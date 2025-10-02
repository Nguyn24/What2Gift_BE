using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Authentication;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Users.UpdateUser;
using What2Gift.Domain.Common;
using What2Gift.Domain.Users.Errors;

namespace What2Gift.Application.Users.UpdateCurrentUser;

public sealed class UpdateCurrentUserCommandHandler(
    IDbContext context,
    IUserContext userContext)
    : ICommandHandler<UpdateCurrentUserCommand, UpdateUserResponse>
{
    public async Task<Result<UpdateUserResponse>> Handle(UpdateCurrentUserCommand request, CancellationToken cancellationToken)
    {
        var userId = userContext.UserId;

        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
        {
            return Result.Failure<UpdateUserResponse>(UserErrors.NotFound(userId));
        }

        user.Username = request.FullName ?? user.Username;
        user.Email = request.Email ?? user.Email;
        user.AvatarUrl = request.AvatarUrl ?? user.AvatarUrl;
        
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(new UpdateUserResponse(user));
    }
}