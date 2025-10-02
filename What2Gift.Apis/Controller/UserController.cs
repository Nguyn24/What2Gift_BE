using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using What2Gift.Apis.Extensions;
using What2Gift.Apis.Requests;
using What2Gift.Application.Abstraction.Authentication;
using What2Gift.Application.Authentication.ChangePassword;
using What2Gift.Application.Authentication.VerifyUser;
using What2Gift.Application.Users.CreateUser;
using What2Gift.Application.Users.GetAllUser;
using What2Gift.Application.Users.GetCurrentUser;
using What2Gift.Application.Users.UpdateCurrentUser;
using What2Gift.Application.Users.UpdateUser;
using What2Gift.Domain.Common;

namespace What2Gift.Apis.Controller;

[Route("api/user/")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IImageUploader _imageUploader;


    public UserController(ISender mediator, IImageUploader imageUploader)
    {
        _mediator = mediator;
        _imageUploader = imageUploader;
    }
    
    // [Authorize(Roles = "Staff")]
    [HttpPost("create-user")]
    public async Task<IResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        CreateUserCommand command = new CreateUserCommand
        {
            Email = request.Email,
            Password = request.Password,
            Name = request.Name,
            Role = request.Role,
        };
        
        Result<CreateUserCommandResponse> result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }
    
    // [Authorize(Roles = "Staff")]
    [HttpGet("get-all-users")]
    public async Task<IResult> GetUsers([FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken cancellation)
    {
        Result<Page<GetUsersResponse>> result = await _mediator.Send(new GetUsersQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
        }, cancellation);
        return result.MatchOk();
    }
    
    [Authorize]
    [HttpPut("update-current-user")]
    public async Task<IResult> UpdateMyProfile([FromBody] UpdateCurrentUserRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCurrentUserCommand()
        {
             FullName = request.FullName,
             Email = request.Email,
             AvatarUrl = request.Image
        };
        
        var result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }
    
    
    [Authorize(Roles = "Staff")]
    [HttpPut("update-user")]
    public async Task<IResult> UpdateByStaff( [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand
        {
            UserId = request.UserId,
            FullName = request.FullName,
            Email = request.Email,
            Role = request.Role,
            Status = request.Status,
        };

        var result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }
    
    [Authorize]
    [HttpGet("get-current-users")]
    public async Task<IResult> GetCurrentUsers( CancellationToken cancellation)
    {
        var query = new GetCurrentUserQuery();
        var result = await _mediator.Send(query, cancellation);
        return result.MatchOk();
    }
    
    
}