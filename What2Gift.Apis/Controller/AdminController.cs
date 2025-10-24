using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using What2Gift.Apis.Extensions;
using What2Gift.Application.Admin.Analytics.GetDashboardStats;
using What2Gift.Application.Admin.Memberships.ExtendMembership;
using What2Gift.Application.Admin.Memberships.GetAllMemberships;
using What2Gift.Application.Admin.Payments.GetAllPayments;
using What2Gift.Application.Admin.Users.GetAllUsers;
using What2Gift.Application.Admin.Users.ToggleUserStatus;
using What2Gift.Domain.Common;

namespace What2Gift.Apis.Controller;

[Route("api/admin/")]
[ApiController]
[Authorize] // TODO: Add admin role authorization
public class AdminController : ControllerBase
{
    private readonly ISender _mediator;

    public AdminController(ISender mediator)
    {
        _mediator = mediator;
    }

    // Dashboard Analytics
    [HttpGet("dashboard/stats")]
    public async Task<IResult> GetDashboardStats(
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        CancellationToken cancellationToken)
    {
        var query = new GetDashboardStatsQuery
        {
            FromDate = fromDate,
            ToDate = toDate
        };

        Result<DashboardStatsResponse> result = await _mediator.Send(query, cancellationToken);
        return result.MatchOk();
    }

    // User Management
    [HttpGet("users")]
    public async Task<IResult> GetAllUsers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? status = null,
        [FromQuery] DateTime? createdFrom = null,
        [FromQuery] DateTime? createdTo = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllUsersQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            Status = status.HasValue ? (Domain.Users.UserStatus)status.Value : null,
            CreatedFrom = createdFrom,
            CreatedTo = createdTo
        };

        Result<Page<AdminUserResponse>> result = await _mediator.Send(query, cancellationToken);
        return result.MatchOk();
    }

    [HttpPut("users/{userId}/toggle-status")]
    public async Task<IResult> ToggleUserStatus(
        [FromRoute] Guid userId,
        [FromBody] ToggleUserStatusRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ToggleUserStatusCommand
        {
            UserId = userId,
            Status = (Domain.Users.UserStatus)request.Status
        };

        Result result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }

    // Membership Management
    [HttpGet("memberships")]
    public async Task<IResult> GetAllMemberships(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] Guid? membershipPlanId = null,
        [FromQuery] DateTime? startDateFrom = null,
        [FromQuery] DateTime? startDateTo = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllMembershipsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            IsActive = isActive,
            MembershipPlanId = membershipPlanId,
            StartDateFrom = startDateFrom,
            StartDateTo = startDateTo
        };

        Result<Page<AdminMembershipResponse>> result = await _mediator.Send(query, cancellationToken);
        return result.MatchOk();
    }

    // Payment Management
    [HttpGet("payments")]
    public async Task<IResult> GetAllPayments(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? status = null,
        [FromQuery] DateTime? createdFrom = null,
        [FromQuery] DateTime? createdTo = null,
        [FromQuery] decimal? minAmount = null,
        [FromQuery] decimal? maxAmount = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllPaymentsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            Status = status.HasValue ? (Domain.Finance.PaymentTransactionStatus)status.Value : null,
            CreatedFrom = createdFrom,
            CreatedTo = createdTo,
            MinAmount = minAmount,
            MaxAmount = maxAmount
        };

        Result<Page<AdminPaymentResponse>> result = await _mediator.Send(query, cancellationToken);
        return result.MatchOk();
    }

    [HttpPut("memberships/{membershipId}/extend")]
    public async Task<IResult> ExtendMembership(
        [FromRoute] Guid membershipId,
        [FromBody] ExtendMembershipRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ExtendMembershipCommand
        {
            MembershipId = membershipId,
            AdditionalDays = request.AdditionalDays,
            Reason = request.Reason
        };

        Result result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }
}

public class ToggleUserStatusRequest
{
    public int Status { get; set; }
}

public class ExtendMembershipRequest
{
    public int AdditionalDays { get; set; }
    public string? Reason { get; set; }
}
