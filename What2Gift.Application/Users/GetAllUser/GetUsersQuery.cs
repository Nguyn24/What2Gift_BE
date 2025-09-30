using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Users.GetAllUser;

public class GetUsersQuery : IPageableQuery, IQuery<Page<GetUsersResponse>>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}