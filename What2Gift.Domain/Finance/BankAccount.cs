using What2Gift.Domain.Common;

namespace What2Gift.Domain.Finance;

public class BankAccount : Entity
{
    public Guid Id { get; set; }
    public string BankName { get; set; } = null!;
    public string AccountNumber { get; set; } = null!;
    public string AccountHolderName { get; set; } = null!;
    public string? QrCodeUrl { get; set; } // URL to QR code image
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}

