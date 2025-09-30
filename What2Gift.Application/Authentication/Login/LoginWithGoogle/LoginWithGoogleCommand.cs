using What2Gift.Application.Abstraction.Messaging;

namespace What2Gift.Application.Authentication.Login.LoginWithGoogle;

public sealed class LoginWithGoogleCommand : ICommand<LoginWithGoogleResponse>
{
    public string IdToken { get; init; } = null!;
}