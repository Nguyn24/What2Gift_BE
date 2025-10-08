using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Occasions.GetAllOccasions;

public class GetAllOccasionsQuery : IPageableQuery, IQuery<Page<OccasionResponse>>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}

public class OccasionResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int StartMonth { get; init; }
    public int StartDay { get; init; }
    public int EndMonth { get; init; }
    public int EndDay { get; init; }
}
