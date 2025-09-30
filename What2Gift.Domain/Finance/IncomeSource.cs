using What2Gift.Domain.Common;

namespace What2Gift.Domain.Finance;

public class IncomeSource : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ICollection<Revenue> Revenues { get; set; } = new List<Revenue>();
}