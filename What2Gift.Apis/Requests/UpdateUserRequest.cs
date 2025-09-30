using What2Gift.Domain.Users;

namespace What2Gift.Apis.Requests;

public class UpdateUserRequest
{
    public Guid UserId { get; set; } 
    public string? FullName { get; set; } 
    public string? Email { get; set; } 
    public UserRole? Role { get; set; }
    public UserStatus? Status { get; set; }
}
