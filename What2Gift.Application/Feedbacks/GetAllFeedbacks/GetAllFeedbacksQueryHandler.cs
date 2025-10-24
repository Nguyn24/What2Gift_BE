using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Feedbacks.GetAllFeedbacks;

public sealed class GetAllFeedbacksQueryHandler(IDbContext context)
    : IQueryHandler<GetAllFeedbacksQuery, Page<GetAllFeedbacksResponse>>
{
    public async Task<Result<Page<GetAllFeedbacksResponse>>> Handle(GetAllFeedbacksQuery request, CancellationToken cancellationToken)
    {
        var feedbacks = await context.Feedbacks
            .Include(f => f.User)
            .OrderByDescending(f => f.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(f => new GetAllFeedbacksResponse
            {
                Id = f.Id,
                UserId = f.UserId,
                UserName = f.User.Username,
                Rating = f.Rating,
                Comment = f.Comment,
                CreatedAt = f.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var totalCount = await context.Feedbacks.CountAsync(cancellationToken);

        var page = new Page<GetAllFeedbacksResponse>(
            feedbacks,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return page;
    }
}
