using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;
using What2Gift.Domain.Finance;

namespace What2Gift.Application.Admin.Payments.GetAllPayments;

public class GetAllPaymentsQuery : IPageableQuery, IQuery<Page<AdminPaymentResponse>>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public string? SearchTerm { get; init; }
    public PaymentTransactionStatus? Status { get; init; }
    public DateTime? CreatedFrom { get; init; }
    public DateTime? CreatedTo { get; init; }
    public decimal? MinAmount { get; init; }
    public decimal? MaxAmount { get; init; }
}

public class AdminPaymentResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string UserEmail { get; init; } = string.Empty;
    public Guid? MembershipPlanId { get; init; }
    public string? MembershipPlanName { get; init; }
    public decimal Amount { get; init; }
    public string PaymentMethod { get; init; } = string.Empty;
    public string TransactionCode { get; init; } = string.Empty;
    public PaymentTransactionStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? PaidAt { get; init; }
    public string? TransactionNo { get; init; }
    public string? BankCode { get; init; }
}
