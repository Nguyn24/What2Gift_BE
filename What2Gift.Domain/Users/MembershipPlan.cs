using What2Gift.Domain.Common;
using What2Gift.Domain.Finance;

namespace What2Gift.Domain.Users;

public class MembershipPlan: Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string? Description { get; set; }
    
    public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();
}