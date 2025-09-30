namespace What2Gift.Application.Authentication.Login.LoginWithGoogle;

public class LoginWithGoogleResponse
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
}