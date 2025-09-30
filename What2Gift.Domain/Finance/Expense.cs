using What2Gift.Domain.Common;

namespace What2Gift.Domain.Finance;

public class Expense : Entity
{
    public Guid Id { get; set; }
    public string Category { get; set; } = null!;
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime SpentAt { get; set; }
}