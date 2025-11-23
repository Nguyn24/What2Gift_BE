using What2Gift.Domain.Common;
using What2Gift.Domain.Users;

namespace What2Gift.Domain.Finance;

public class TopUpTransaction : Entity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; } // Amount in VND
    public int Points { get; set; } // Points to be added (Amount * 1, since 1000 points = 1000 VND)
    public string TransferContent { get; set; } = null!; // Format: nap'xxxx'
    public TopUpTransactionStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public Guid? ApprovedBy { get; set; } // Admin user ID who approved
    public string? Note { get; set; }

    public User User { get; set; } = null!;
    public User? Approver { get; set; }
}

