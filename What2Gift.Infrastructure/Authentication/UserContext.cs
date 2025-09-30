using Microsoft.AspNetCore.Http;
using What2Gift.Application.Abstraction.Authentication;

namespace What2Gift.Infrastructure.Authentication;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public Guid UserId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ??
        throw new ApplicationException("Users context is unavailable");
}
