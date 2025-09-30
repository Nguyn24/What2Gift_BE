using What2Gift.Domain.Users;

namespace What2Gift.Application.Users.CreateUser;

public sealed record CreateUserCommandResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public UserRole Role { get; init; }
}