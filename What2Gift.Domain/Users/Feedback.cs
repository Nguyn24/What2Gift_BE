using What2Gift.Domain.Common;
using What2Gift.Domain.Products;

namespace What2Gift.Domain.Users;

public class Feedback : Entity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
}