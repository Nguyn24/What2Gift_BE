using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Users.UpdateUser;

namespace What2Gift.Application.Users.UpdateCurrentUser;

public sealed class UpdateCurrentUserCommand : ICommand<UpdateUserResponse>
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? AvatarUrl { get; set; }
    
}