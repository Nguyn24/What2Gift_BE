using What2Gift.Application.Abstraction.Messaging;

namespace What2Gift.Application.Authentication.Login;

public sealed class LoginCommand() : ICommand<TokenResponse>
{
    public string Email { get; init; }
    public string Password { get; init; }
}