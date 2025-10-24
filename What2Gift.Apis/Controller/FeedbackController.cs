using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using What2Gift.Apis.Extensions;
using What2Gift.Apis.Requests;
using What2Gift.Application.Abstraction.Authentication;
using What2Gift.Application.Feedbacks.CreateFeedback;
using What2Gift.Application.Feedbacks.DeleteFeedback;
using What2Gift.Application.Feedbacks.GetAllFeedbacks;
using What2Gift.Application.Feedbacks.UpdateFeedback;
using What2Gift.Domain.Common;

namespace What2Gift.Apis.Controller;

[Route("api/feedback/")]
[ApiController]
public class FeedbackController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IUserContext _userContext;

    public FeedbackController(ISender mediator, IUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    [Authorize(Roles = "Member")]
    [HttpPost("create-feedback")]
    public async Task<IResult> CreateFeedback([FromBody] CreateFeedbackRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateFeedbackCommand
        {
            UserId = _userContext.UserId,
            Rating = request.Rating,
            Comment = request.Comment
        };

        Result<CreateFeedbackCommandResponse> result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }

    [HttpGet("get-all-feedbacks")]
    public async Task<IResult> GetAllFeedbacks([FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        Result<Page<GetAllFeedbacksResponse>> result = await _mediator.Send(new GetAllFeedbacksQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
        }, cancellationToken);
        return result.MatchOk();
    }

    [Authorize(Roles = "Staff")]
    [HttpPut("update-feedback")]
    public async Task<IResult> UpdateFeedback([FromBody] UpdateFeedbackRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateFeedbackCommand
        {
            Id = request.Id,
            Rating = request.Rating,
            Comment = request.Comment
        };

        Result<UpdateFeedbackCommandResponse> result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }

    [Authorize(Roles = "Staff")]
    [HttpDelete("delete-feedback")]
    public async Task<IResult> DeleteFeedback([FromBody] DeleteFeedbackRequest request, CancellationToken cancellationToken)
    {
        var command = new DeleteFeedbackCommand
        {
            Id = request.Id
        };

        Result result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }
}
