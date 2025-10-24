using MediatR;
using Microsoft.AspNetCore.Mvc;
using What2Gift.Apis.Extensions;
using What2Gift.Apis.Requests;
using What2Gift.Application.MembershipPlans.CreateMembershipPlan;
using What2Gift.Application.MembershipPlans.DeleteMembershipPlan;
using What2Gift.Application.MembershipPlans.GetAllMembershipPlans;
using What2Gift.Application.MembershipPlans.UpdateMembershipPlan;
using What2Gift.Domain.Common;

namespace What2Gift.Apis.Controller;

[Route("api/membership-plan/")]
[ApiController]
public class MembershipPlanController : ControllerBase
{
    private readonly ISender _mediator;

    public MembershipPlanController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IResult> CreateMembershipPlan([FromBody] CreateMembershipPlanRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateMembershipPlanCommand
        {
            Name = request.Name,
            Price = request.Price,
            Description = request.Description
        };

        Result result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }

    [HttpGet("get-all")]
    public async Task<IResult> GetAllMembershipPlans([FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        Result<Page<MembershipPlanResponse>> result = await _mediator.Send(new GetAllMembershipPlansQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
        }, cancellationToken);
        return result.MatchOk();
    }

    [HttpPut("update")]
    public async Task<IResult> UpdateMembershipPlan([FromBody] UpdateMembershipPlanRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateMembershipPlanCommand
        {
            Id = request.Id,
            Name = request.Name,
            Price = request.Price,
            Description = request.Description
        };

        Result result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }

    [HttpDelete("delete")]
    public async Task<IResult> DeleteMembershipPlan([FromBody] DeleteMembershipPlanRequest request, CancellationToken cancellationToken)
    {
        var command = new DeleteMembershipPlanCommand
        {
            Id = request.Id
        };

        Result result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }
}
