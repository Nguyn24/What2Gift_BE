using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Authentication;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Users.Helpers;
using What2Gift.Domain.Common;
using What2Gift.Domain.Users;
using What2Gift.Domain.Users.Errors;

namespace What2Gift.Application.Authentication.Register;

public class RegisterCommandHandler(
    IDbContext context,
    IPasswordHasher passwordHasher
) : ICommandHandler<RegisterCommand, RegisterResponse>
{
    public async Task<Result<RegisterResponse>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        if (await context.Users.AnyAsync(u => u.Email.Trim().ToLower() == command.Email.Trim().ToLower(), cancellationToken))
        {
            return Result.Failure<RegisterResponse>(UserErrors.EmailNotUnique);
        }

        var hashedPassword = passwordHasher.Hash(command.Password);
        
        // Generate unique TopUpCode (0-9999)
        var topUpCode = await TopUpCodeGenerator.GenerateUniqueTopUpCodeAsync(context, cancellationToken);
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = command.Name,
            Email = command.Email,
            Password = hashedPassword,
            Role = UserRole.Member,
            Status = UserStatus.Active,
            CreatedAt = DateTime.UtcNow,
            IsVerified = false,
            MembershipStatus = MembershipStatus.Inactive,
            TopUpCode = topUpCode
        };

        // user.Raise(new UserCreatedDomainEvent(user.UserId));
        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        return new RegisterResponse
        {
            Name = user.Username,
            Email = user.Email,
        };
    }
}