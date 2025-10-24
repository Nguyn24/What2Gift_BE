using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Application.Abstraction.Query;
using What2Gift.Domain.Common;
using What2Gift.Domain.Users;

namespace What2Gift.Application.Admin.Users.GetAllUsers;

public class GetAllUsersQuery : IPageableQuery, IQuery<Page<AdminUserResponse>>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public string? SearchTerm { get; init; }
    public UserStatus? Status { get; init; }
    public DateTime? CreatedFrom { get; init; }
    public DateTime? CreatedTo { get; init; }
}

public class AdminUserResponse
{
    public Guid Id { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public UserStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public bool IsVerified { get; init; }
    public bool HasActiveMembership { get; init; }
    public string? MembershipPlanName { get; init; }
    public DateTime? MembershipExpiresAt { get; init; }
    public int TotalOrders { get; init; }
    public decimal TotalSpent { get; init; }
}
