using What2Gift.Domain.Users;

namespace What2Gift.Application.Users.GetAllUser;

public sealed class GetUsersResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public UserRole Role { get; set; }
    public MembershipStatus MembershipStatus { get; set; }
    public UserStatus Status { get; set; }
    public Membership? Membership { get; set; }

}
public class Membership
{
    public MembershipType MembershipType { get; set; } 
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}

