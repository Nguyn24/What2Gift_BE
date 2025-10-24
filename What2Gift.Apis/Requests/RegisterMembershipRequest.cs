namespace What2Gift.Apis.Requests;

public class RegisterMembershipRequest
{
    public Guid MembershipPlanId { get; set; }
    public string ReturnUrl { get; set; } = string.Empty;
}
