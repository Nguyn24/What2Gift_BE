using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;
using What2Gift.Domain.Finance;

namespace What2Gift.Application.Admin.TopUp.GetAllTopUpTransactions;

public class GetAllTopUpTransactionsQueryHandler(IDbContext context) 
    : IQueryHandler<GetAllTopUpTransactionsQuery, Page<AdminTopUpTransactionResponse>>
{
    public async Task<Result<Page<AdminTopUpTransactionResponse>>> Handle(
        GetAllTopUpTransactionsQuery request, 
        CancellationToken cancellationToken)
    {
        // Only get pending top-up transactions
        var query = context.TopUpTransactions
            .Include(t => t.User)
            .Include(t => t.Approver)
            .Where(t => t.Status == TopUpTransactionStatus.Pending)
            .AsQueryable();

        // Note: Status filter is ignored - only pending transactions are shown

        if (request.CreatedFrom.HasValue)
        {
            query = query.Where(t => t.CreatedAt >= request.CreatedFrom.Value);
        }

        if (request.CreatedTo.HasValue)
        {
            query = query.Where(t => t.CreatedAt <= request.CreatedTo.Value);
        }

        if (request.MinAmount.HasValue)
        {
            query = query.Where(t => t.Amount >= request.MinAmount.Value);
        }

        if (request.MaxAmount.HasValue)
        {
            query = query.Where(t => t.Amount <= request.MaxAmount.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            query = query.Where(t => 
                t.TransferContent.ToLower().Contains(searchTerm) ||
                t.User.Username.ToLower().Contains(searchTerm) ||
                t.User.Email.ToLower().Contains(searchTerm));
        }

        // Apply sorting
        query = request.SortBy?.ToLower() switch
        {
            "amount" => request.SortOrder == SortOrder.Ascending 
                ? query.OrderBy(t => t.Amount) 
                : query.OrderByDescending(t => t.Amount),
            "createdat" => request.SortOrder == SortOrder.Ascending 
                ? query.OrderBy(t => t.CreatedAt) 
                : query.OrderByDescending(t => t.CreatedAt),
            "status" => request.SortOrder == SortOrder.Ascending 
                ? query.OrderBy(t => t.Status) 
                : query.OrderByDescending(t => t.Status),
            _ => query.OrderByDescending(t => t.CreatedAt)
        };

        // Get total count
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination
        var transactions = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(t => new AdminTopUpTransactionResponse
            {
                Id = t.Id,
                UserId = t.UserId,
                Username = t.User.Username,
                Email = t.User.Email,
                Amount = t.Amount,
                Points = t.Points,
                TransferContent = t.TransferContent,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                ApprovedAt = t.ApprovedAt,
                ApprovedBy = t.ApprovedBy,
                Note = t.Note
            })
            .ToListAsync(cancellationToken);

        return Result.Success(new Page<AdminTopUpTransactionResponse>(
            transactions,
            totalCount,
            request.PageNumber,
            request.PageSize));
    }
}

