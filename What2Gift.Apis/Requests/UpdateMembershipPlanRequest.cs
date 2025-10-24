namespace What2Gift.Apis.Requests;

public class UpdateMembershipPlanRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
}
