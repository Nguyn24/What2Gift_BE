using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using What2Gift.Apis.Extensions;
using What2Gift.Apis.Requests;
using What2Gift.Application.Authentication.ChangePassword;
using What2Gift.Application.Authentication.Login;
using What2Gift.Application.Authentication.Login.LoginWithGoogle;
using What2Gift.Application.Authentication.Register;
using What2Gift.Application.Authentication.VerifyUser;

// using BloodDonation.Application.Users.ResetPassword;

namespace What2Gift.Apis.Controller;


[Route("api/auth/")]
[ApiController]
public class AuthenticateController : ControllerBase
{
    private readonly ISender _mediator;

    public AuthenticateController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    
    [HttpPost("login")]
    public async Task<IResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        LoginCommand command = new LoginCommand
        {
            Email = request.Email,
            Password = request.Password
        };
        
        var  result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }
    
    [HttpPost("loginWithRefreshToken")]
    public async Task<IResult> LoginWithRefreshToken([FromBody] string refreshToken, CancellationToken cancellationToken)
    {
        LoginWithRefreshToken.LoginByRefreshTokenCommand command = new LoginWithRefreshToken.LoginByRefreshTokenCommand
        {
            RefreshToken = refreshToken
        };
        
        var  result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }
    
    [HttpPost("register")]
    public async Task<IResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        RegisterCommand command = new RegisterCommand
        {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password,
        };
        
        var result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }
    
    [Authorize]
    [HttpPut("change-password")]
    public async Task<IResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var command = new ChangePasswordCommand
        {
            CurrentPassword = request.CurrentPassword,
            NewPassword = request.NewPassword
        };

        var result = await _mediator.Send(command, cancellationToken);

        return result.MatchOk();
    }
    
    [HttpPut("verify")]
    public async Task<IResult> VerifyUser([FromBody] VerifyUserRequest request, CancellationToken cancellationToken)
    {
        var command = new VerifyUserCommand(request.Token);
        var result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }
    
    // [HttpPost("auth/forget-password")]
    // public async Task<IResult> ForgetPassword([FromBody] ForgetPasswordRequest request, CancellationToken cancellationToken)
    // {
    //     var command = new ForgetPasswordCommand()
    //     {
    //         Email = request.Email
    //     };
    //
    //     var result = await _mediator.Send(command, cancellationToken);
    //     return result.MatchOk();
    // }

    [HttpPost("loginWithGoogle")]
    public async Task<IResult> LoginWithGoogle([FromBody] LoginWithGoogleRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginWithGoogleCommand()
        {
            IdToken = request.IdToken
        };
    
        var result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }

}