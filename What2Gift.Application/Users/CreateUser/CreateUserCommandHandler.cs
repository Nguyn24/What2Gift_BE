using BloodDonation.Application.Users.CreateUser;
using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Authentication;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Users.Helpers;
using What2Gift.Domain.Common;
using What2Gift.Domain.Users;
using What2Gift.Domain.Users.Errors;

namespace What2Gift.Application.Users.CreateUser;

public class CreateUserCommandHandler(
    IDbContext context,
    IPasswordHasher passwordHasher
) : ICommandHandler<CreateUserCommand, CreateUserCommandResponse>
{
    public async Task<Result<CreateUserCommandResponse>> Handle(CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        if (await context.Users.AnyAsync(u => u.Email.Trim().ToLower() == command.Email.Trim().ToLower(),
                cancellationToken))
        {
            return Result.Failure<CreateUserCommandResponse>(UserErrors.EmailNotUnique);
        }

        if (command.Role == UserRole.Admin)
        {
            bool adminExists = await context.Users
                .AnyAsync(u => u.Role == UserRole.Admin, cancellationToken);

            if (adminExists)
            {
                return Result.Failure<CreateUserCommandResponse>(UserErrors.AdminAlreadyExists);
            }
        }

        var hashedPassword = passwordHasher.Hash(command.Password);
        
        // Generate unique TopUpCode (0-9999) for regular users, null for admin
        int? topUpCode = null;
        if (command.Role != UserRole.Admin)
        {
            topUpCode = await TopUpCodeGenerator.GenerateUniqueTopUpCodeAsync(context, cancellationToken);
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = command.Name,
            Email = command.Email,
            Password = hashedPassword,
            Role = command.Role,
            Status = UserStatus.Active,
            IsVerified = false,
            CreatedAt = DateTime.UtcNow,
            MembershipStatus = MembershipStatus.Inactive,
            TopUpCode = topUpCode
        };

        // user.Raise(new UserCreatedDomainEvent(user.UserId));


        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateUserCommandResponse
        {
            Id = user.Id,
            Name = user.Username,
            Email = user.Email,
            Role = user.Role
        };
    }
}