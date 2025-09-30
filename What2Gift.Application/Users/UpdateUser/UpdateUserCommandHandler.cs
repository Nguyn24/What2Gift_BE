using MediatR;
using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Authentication;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Users.Errors;

namespace What2Gift.Application.Users.UpdateUser;

public sealed class UpdateUserCommandHandler(IDbContext context, IUserContext userContext, ISender sender)
    : ICommandHandler<UpdateUserCommand, UpdateUserResponse>
{
    public async Task<Result<UpdateUserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        if (user == null)
        {
            return Result.Failure<UpdateUserResponse>(UserErrors.NotFound(request.UserId));
        }
        
        // var currentUserResult = await sender.Send(new GetCurrentUserQuery(), cancellationToken);
        // var currentUser = currentUserResult.Value;
        // bool isStaff = currentUser.Role == UserRole.Staff.ToString();
        

        user.Username = request.FullName ?? user.Username;
        user.Email = request.Email ?? user.Email;

        // if (isStaff)
        // 
            user.Role = request.Role ?? user.Role;
            user.Status = request.Status ?? user.Status;
        // }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(new UpdateUserResponse(user));
    }
}
