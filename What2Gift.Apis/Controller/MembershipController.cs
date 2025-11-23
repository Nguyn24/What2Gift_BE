using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using What2Gift.Apis.Extensions;
using What2Gift.Apis.Requests;
using What2Gift.Application.Abstraction.Authentication;
using What2Gift.Application.Memberships.RegisterMembership;
using What2Gift.Domain.Common;

namespace What2Gift.Apis.Controller;

[Route("api/membership/")]
[ApiController]
public class MembershipController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IUserContext _userContext;

    public MembershipController(ISender mediator, IUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    [HttpPost("register")]
    [Authorize]
    public async Task<IResult> RegisterMembership([FromBody] RegisterMembershipRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterMembershipCommand
        {
            UserId = _userContext.UserId,
            MembershipPlanId = request.MembershipPlanId
        };

        Result<RegisterMembershipResponse> result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }
}
