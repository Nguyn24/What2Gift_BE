using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;
using What2Gift.Domain.Finance;

namespace What2Gift.Application.Admin.TopUp.GetAllTopUpTransactions;

public class GetAllTopUpTransactionsQuery : IQuery<Page<AdminTopUpTransactionResponse>>, IPageableQuery, ISortableQuery
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchTerm { get; init; }
    // Note: Only pending transactions are shown, status filter removed
    public DateTime? CreatedFrom { get; init; }
    public DateTime? CreatedTo { get; init; }
    public decimal? MinAmount { get; init; }
    public decimal? MaxAmount { get; init; }
    public string? SortBy { get; init; }
    public SortOrder SortOrder { get; init; } = SortOrder.Descending;
}

public class AdminTopUpTransactionResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
    public decimal Amount { get; init; }
    public int Points { get; init; }
    public string TransferContent { get; init; } = null!;
    public TopUpTransactionStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? ApprovedAt { get; init; }
    public Guid? ApprovedBy { get; init; }
    public string? Note { get; init; }
}

