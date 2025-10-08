using MediatR;
using Microsoft.AspNetCore.Mvc;
using What2Gift.Apis.Extensions;
using What2Gift.Apis.Requests;
using What2Gift.Application.Occasions.CreateOccasion;
using What2Gift.Application.Occasions.DeleteOccasion;
using What2Gift.Application.Occasions.GetAllOccasions;
using What2Gift.Application.Occasions.UpdateOccasion;
using What2Gift.Domain.Common;

namespace What2Gift.Apis.Controller;

[Route("api/occasion/")]
[ApiController]
public class OccasionController : ControllerBase
{
    private readonly ISender _mediator;

    public OccasionController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create-occasion")]
    public async Task<IResult> CreateOccasion([FromBody] CreateOccasionRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateOccasionCommand
        {
            Name = request.Name,
            StartMonth = request.StartMonth,
            StartDay = request.StartDay,
            EndMonth = request.EndMonth,
            EndDay = request.EndDay
        };

        Result result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }

    [HttpGet("get-all-occasions")]
    public async Task<IResult> GetAllOccasions([FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        Result<Page<OccasionResponse>> result = await _mediator.Send(new GetAllOccasionsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
        }, cancellationToken);
        return result.MatchOk();
    }

    [HttpPut("update-occasion")]
    public async Task<IResult> UpdateOccasion([FromBody] UpdateOccasionRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateOccasionCommand
        {
            Id = request.Id,
            Name = request.Name,
            StartMonth = request.StartMonth,
            StartDay = request.StartDay,
            EndMonth = request.EndMonth,
            EndDay = request.EndDay
        };

        Result result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }

    [HttpDelete("delete-occasion")]
    public async Task<IResult> DeleteOccasion([FromBody] DeleteOccasionRequest request, CancellationToken cancellationToken)
    {
        var command = new DeleteOccasionCommand
        {
            Id = request.Id
        };

        Result result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }
}
