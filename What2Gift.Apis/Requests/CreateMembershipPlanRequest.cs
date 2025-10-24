namespace What2Gift.Apis.Requests;

public class CreateMembershipPlanRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
}
