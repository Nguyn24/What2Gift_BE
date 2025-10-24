using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Admin.Users.ToggleUserStatus;

public class ToggleUserStatusCommandHandler(IDbContext context) : ICommandHandler<ToggleUserStatusCommand>
{
    public async Task<Result> Handle(ToggleUserStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(Error.NotFound("User.NotFound", "User not found"));
        }

        user.Status = request.Status;
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
