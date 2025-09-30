using What2Gift.Application.Abstraction.Messaging;

namespace What2Gift.Application.Authentication.VerifyUser;

public record VerifyUserCommand(string Token) : ICommand
{
    
}