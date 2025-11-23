using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;

namespace What2Gift.Application.TopUp.GetTopUpInfo;

public class GetTopUpInfoQuery : IQuery<GetTopUpInfoResponse>
{
    public Guid UserId { get; init; }
}

public class GetTopUpInfoResponse
{
    public string TransferContent { get; init; } = null!; // Format: nap'xxxx'
    public string BankAccountNumber { get; init; } = null!;
    public string BankName { get; init; } = null!;
    public string AccountHolderName { get; init; } = null!;
    public string? QrCodeUrl { get; init; } // URL to QR code image (null if not uploaded yet)
    public int CurrentPoints { get; init; }
}

