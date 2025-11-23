using What2Gift.Domain.Common;
using What2Gift.Domain.Users;

namespace What2Gift.Domain.Finance;

public class PaymentTransaction : Entity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? MembershipPlanId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = "W2G_POINTS";
    public string TransactionCode { get; set; } = null!;
    public PaymentTransactionStatus Status { get; set; } 
    public DateTime CreatedAt { get; set; }
    public DateTime? PaidAt { get; set; }

    public User User { get; set; } = null!;
    public MembershipPlan? MembershipPlan { get; set; }
}