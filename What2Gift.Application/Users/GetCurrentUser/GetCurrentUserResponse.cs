using What2Gift.Domain.Users;

namespace What2Gift.Application.Users.GetCurrentUser
{
    public sealed record GetCurrentUserResponse
    {
        // public string? ImageUrl { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
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
}
