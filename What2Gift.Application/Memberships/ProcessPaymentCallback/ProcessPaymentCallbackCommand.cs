using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Memberships.ProcessPaymentCallback;

public class ProcessPaymentCallbackCommand : ICommand<ProcessPaymentCallbackResponse>
{
    public string TransactionId { get; init; } = string.Empty;
    public bool IsSuccess { get; init; }
    public decimal Amount { get; set; }
    public string TransactionNo { get; init; } = string.Empty;
    public string BankCode { get; init; } = string.Empty;
    public string PayDate { get; init; } = string.Empty;
}

public class ProcessPaymentCallbackResponse
{
    public bool IsSuccess { get; init; }
    public string Message { get; init; } = string.Empty;
    public Guid? MembershipId { get; init; }
}
