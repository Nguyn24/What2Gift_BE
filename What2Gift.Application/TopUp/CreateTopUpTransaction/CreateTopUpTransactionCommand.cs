using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.TopUp.CreateTopUpTransaction;

public class CreateTopUpTransactionCommand : ICommand<CreateTopUpTransactionResponse>
{
    public Guid UserId { get; init; }
    public decimal Amount { get; init; } // Amount in VND
}

public class CreateTopUpTransactionResponse
{
    public Guid TopUpTransactionId { get; init; }
    public string TransferContent { get; init; } = null!;
    public decimal Amount { get; init; }
    public int Points { get; init; }
}

