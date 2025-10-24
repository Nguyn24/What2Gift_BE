using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Users;

namespace What2Gift.Application.Admin.Users.ToggleUserStatus;

public class ToggleUserStatusCommand : ICommand
{
    public Guid UserId { get; init; }
    public UserStatus Status { get; init; }
}
