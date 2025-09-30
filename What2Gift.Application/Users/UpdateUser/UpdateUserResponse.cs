using What2Gift.Domain.Users;

namespace What2Gift.Application.Users.UpdateUser;

public class UpdateUserResponse
{
    public Guid Id { get; init; }
    public string FullName { get; init; }
    public string Email { get; init; }
    public UserRole Role { get; init; }
    public UserStatus Status { get; init; }

    public UpdateUserResponse(User user)
    {
        Id = user.Id;
        FullName = user.Username;
        Email = user.Email;
        Role = user.Role;
        Status = user.Status;
      
    }
}