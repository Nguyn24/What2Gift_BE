using What2Gift.Domain.Common;

namespace What2Gift.Domain.Users;

public class Membership : Entity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public MembershipType MembershipType { get; set; } 
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public User User { get; set; } = null!;
}