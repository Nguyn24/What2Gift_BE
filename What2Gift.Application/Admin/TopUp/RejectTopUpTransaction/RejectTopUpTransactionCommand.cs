using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Admin.TopUp.RejectTopUpTransaction;

public class RejectTopUpTransactionCommand : ICommand<RejectTopUpTransactionResponse>
{
    public Guid TopUpTransactionId { get; init; }
    public Guid AdminUserId { get; init; }
    public string? Note { get; init; }
}

public class RejectTopUpTransactionResponse
{
    public bool IsSuccess { get; init; }
    public string Message { get; init; } = null!;
}

