using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Feedbacks.GetAllFeedbacks;

public sealed class GetAllFeedbacksQuery : IQuery<Page<GetAllFeedbacksResponse>>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}
