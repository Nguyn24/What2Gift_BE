using What2Gift.Domain.Common;

namespace What2Gift.Domain.Finance;

public class Revenue : Entity
{
    public Guid Id { get; set; }
    public Guid IncomeSourceId { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime ReceivedAt { get; set; }
    public IncomeSource IncomeSource { get; set; } = null!;
}