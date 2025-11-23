using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Admin.TopUp.ApproveTopUpTransaction;

public class ApproveTopUpTransactionCommand : ICommand<ApproveTopUpTransactionResponse>
{
    public Guid TopUpTransactionId { get; init; }
    public Guid AdminUserId { get; init; }
    public string? Note { get; init; }
}

public class ApproveTopUpTransactionResponse
{
    public bool IsSuccess { get; init; }
    public string Message { get; init; } = null!;
    public int PointsAdded { get; init; }
    public int NewBalance { get; init; }
}

