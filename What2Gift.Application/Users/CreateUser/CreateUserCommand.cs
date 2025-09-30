using BloodDonation.Application.Users.CreateUser;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Users;

namespace What2Gift.Application.Users.CreateUser;

public sealed class CreateUserCommand : ICommand<CreateUserCommandResponse>
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public UserRole Role { get; init; }
}