using What2Gift.Domain.Common;
using What2Gift.Domain.Products;
using What2Gift.Domain.Users;

namespace What2Gift.Domain.Affiliate;

public class AffiliateClick : Entity
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public Guid ProductSourceId { get; set; }
    public DateTime ClickedAt { get; set; }
    public string UserAgent { get; set; } = null!;
    public string IpAddress { get; set; } = null!;

    public User? User { get; set; }
    public ProductSource ProductSource { get; set; } = null!;
}