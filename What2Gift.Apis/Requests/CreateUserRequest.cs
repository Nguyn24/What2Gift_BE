using What2Gift.Domain.Users;

namespace What2Gift.Apis.Requests;

public class CreateUserRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
}