using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using What2Gift.Apis.Extensions;
using What2Gift.Apis.Requests;
using What2Gift.Application.Abstraction.Authentication;
using What2Gift.Application.Memberships.ProcessPaymentCallback;
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
            MembershipPlanId = request.MembershipPlanId,
            ReturnUrl = request.ReturnUrl
        };

        Result<RegisterMembershipResponse> result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }

    [HttpPost("payment-callback")]
    public async Task<IResult> ProcessPaymentCallback([FromQuery] Dictionary<string, string> queryParams, CancellationToken cancellationToken)
    {
        // Extract VNPAY response parameters
        var transactionId = queryParams.GetValueOrDefault("vnp_TxnRef", "");
        var responseCode = queryParams.GetValueOrDefault("vnp_ResponseCode", "");
        var amount = decimal.Parse(queryParams.GetValueOrDefault("vnp_Amount", "0")) / 100; // Convert from VND
        var transactionNo = queryParams.GetValueOrDefault("vnp_TransactionNo", "");
        var bankCode = queryParams.GetValueOrDefault("vnp_BankCode", "");
        var payDate = queryParams.GetValueOrDefault("vnp_PayDate", "");

        var command = new ProcessPaymentCallbackCommand
        {
            TransactionId = transactionId,
            IsSuccess = responseCode == "00",
            Amount = amount,
            TransactionNo = transactionNo,
            BankCode = bankCode,
            PayDate = payDate
        };

        Result<ProcessPaymentCallbackResponse> result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }
}
