using What2Gift.Application.Abstraction.Messaging;

namespace What2Gift.Application.Authentication.Register;

public class RegisterCommand : ICommand<RegisterResponse>
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
}