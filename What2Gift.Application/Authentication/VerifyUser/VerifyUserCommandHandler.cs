using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Users.Errors;

namespace What2Gift.Application.Authentication.VerifyUser;

public class VerifyUserCommandHandler(IDbContext context)
    : ICommandHandler<VerifyUserCommand>
{
    public async Task<Result> Handle(VerifyUserCommand command, CancellationToken cancellationToken)
    {
        var email = VerifyTokenHelper.Decode(command.Token);

        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        if (user == null)
            return Result.Failure(UserErrors.NotFoundByEmail);;

        if (!user.IsVerified)
        {
            user.IsVerified = true;
            await context.SaveChangesAsync(cancellationToken);
        }

        return Result.Success();
    }
}