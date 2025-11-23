using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Admin.Payments.GetAllPayments;

public class GetAllPaymentsQueryHandler(IDbContext context) : IQueryHandler<GetAllPaymentsQuery, Page<AdminPaymentResponse>>
{
    public async Task<Result<Page<AdminPaymentResponse>>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
    {
        var query = context.PaymentTransactions
            .Include(pt => pt.User)
            .Include(pt => pt.MembershipPlan)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(pt => 
                pt.User.Username.Contains(request.SearchTerm) || 
                pt.User.Email.Contains(request.SearchTerm) ||
                pt.TransactionCode.Contains(request.SearchTerm) ||
                (pt.MembershipPlan != null && pt.MembershipPlan.Name.Contains(request.SearchTerm)));
        }

        if (request.Status.HasValue)
        {
            query = query.Where(pt => pt.Status == request.Status.Value);
        }

        if (request.CreatedFrom.HasValue)
        {
            query = query.Where(pt => pt.CreatedAt >= request.CreatedFrom.Value);
        }

        if (request.CreatedTo.HasValue)
        {
            query = query.Where(pt => pt.CreatedAt <= request.CreatedTo.Value);
        }

        if (request.MinAmount.HasValue)
        {
            query = query.Where(pt => pt.Amount >= request.MinAmount.Value);
        }

        if (request.MaxAmount.HasValue)
        {
            query = query.Where(pt => pt.Amount <= request.MaxAmount.Value);
        }

        // Filter by PaymentType
        if (!string.IsNullOrEmpty(request.PaymentType))
        {
            var paymentTypeUpper = request.PaymentType.ToUpper();
            query = paymentTypeUpper switch
            {
                "TOP_UP" => query.Where(pt => pt.MembershipPlanId == null),
                "MEMBERSHIP" => query.Where(pt => pt.MembershipPlanId != null),
                _ => query
            };
        }

        // Apply sorting
        query = request.SortBy?.ToLower() switch
        {
            "amount" => request.SortOrder == SortOrder.Ascending 
                ? query.OrderBy(pt => pt.Amount) 
                : query.OrderByDescending(pt => pt.Amount),
            "createdat" => request.SortOrder == SortOrder.Ascending 
                ? query.OrderBy(pt => pt.CreatedAt) 
                : query.OrderByDescending(pt => pt.CreatedAt),
            "status" => request.SortOrder == SortOrder.Ascending 
                ? query.OrderBy(pt => pt.Status) 
                : query.OrderByDescending(pt => pt.Status),
            "paymentmethod" => request.SortOrder == SortOrder.Ascending 
                ? query.OrderBy(pt => pt.PaymentMethod) 
                : query.OrderByDescending(pt => pt.PaymentMethod),
            "paymenttype" => request.SortOrder == SortOrder.Ascending
                ? query.OrderBy(pt => pt.MembershipPlanId.HasValue ? 1 : 0) // MEMBERSHIP = 1, TOP_UP = 0
                : query.OrderByDescending(pt => pt.MembershipPlanId.HasValue ? 1 : 0),
            _ => query.OrderByDescending(pt => pt.CreatedAt)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var result = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(pt => new AdminPaymentResponse
            {
                Id = pt.Id,
                UserId = pt.UserId,
                Username = pt.User.Username,
                UserEmail = pt.User.Email,
                MembershipPlanId = pt.MembershipPlanId,
                MembershipPlanName = pt.MembershipPlan != null ? pt.MembershipPlan.Name : null,
                Amount = pt.Amount,
                PaymentMethod = pt.PaymentMethod,
                TransactionCode = pt.TransactionCode,
                Status = pt.Status,
                CreatedAt = pt.CreatedAt,
                PaidAt = pt.PaidAt,
                PaymentType = pt.MembershipPlanId != null ? "MEMBERSHIP" : "TOP_UP"
            })
            .ToListAsync(cancellationToken);

        return new Page<AdminPaymentResponse>(
            result,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}
