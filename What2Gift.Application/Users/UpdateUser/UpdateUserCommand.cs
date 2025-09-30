using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Users;

namespace What2Gift.Application.Users.UpdateUser;

public sealed class UpdateUserCommand : ICommand<UpdateUserResponse>
{
    public Guid UserId { get; set; } 
    public string? FullName { get; set; } 
    public string? Email { get; set; } 
    public UserRole? Role { get; set; }
    public UserStatus? Status { get; set; }
}


