using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using What2Gift.Apis.Extensions;
using What2Gift.Apis.Requests;
using What2Gift.Application.Abstraction.Authentication;
using What2Gift.Application.TopUp.CreateTopUpTransaction;
using What2Gift.Application.TopUp.GetTopUpInfo;
using What2Gift.Domain.Common;

namespace What2Gift.Apis.Controller;

[Route("api/topup/")]
[ApiController]
public class TopUpController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IUserContext _userContext;

    public TopUpController(ISender mediator, IUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    [HttpGet("info")]
    [Authorize]
    public async Task<IResult> GetTopUpInfo(CancellationToken cancellationToken)
    {
        var query = new GetTopUpInfoQuery
        {
            UserId = _userContext.UserId
        };

        Result<GetTopUpInfoResponse> result = await _mediator.Send(query, cancellationToken);
        return result.MatchOk();
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<IResult> CreateTopUpTransaction([FromBody] CreateTopUpTransactionRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateTopUpTransactionCommand
        {
            UserId = _userContext.UserId,
            Amount = request.Amount
        };

        Result<CreateTopUpTransactionResponse> result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }
}

