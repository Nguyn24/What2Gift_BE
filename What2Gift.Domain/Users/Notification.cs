using What2Gift.Domain.Common;

namespace What2Gift.Domain.Users;

public class Notification : Entity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
}