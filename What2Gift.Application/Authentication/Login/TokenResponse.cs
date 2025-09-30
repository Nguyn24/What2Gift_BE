using What2Gift.Domain.Users;

namespace What2Gift.Application.Authentication.Login;

public class TokenResponse
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
    public UserRole Role { get; set; }
    public Guid UserId { get; set; }

}