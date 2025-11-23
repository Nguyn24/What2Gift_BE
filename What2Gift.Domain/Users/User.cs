using What2Gift.Domain.Affiliate;
using What2Gift.Domain.Common;
using What2Gift.Domain.Finance;
using What2Gift.Domain.Products;

namespace What2Gift.Domain.Users;

public class User : Entity
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public UserRole Role { get; set; } 
    public DateTime CreatedAt { get; set; }
    public UserStatus Status { get; set; } 
    public bool IsVerified { get; set; }
    public MembershipStatus MembershipStatus { get; set; }
    public string? AvatarUrl { get; set; }
    public int W2GPoints { get; set; } = 0; // 1000 points = 1000 VND
    public int? TopUpCode { get; set; } // Random number from 0-9999 for transfer content (nap0000 - nap9999)
    public Membership? Membership { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<GiftSuggestion> GiftSuggestions { get; set; } = new List<GiftSuggestion>();
    public ICollection<AffiliateClick> AffiliateClicks { get; set; } = new List<AffiliateClick>();
    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();
    public ICollection<TopUpTransaction> TopUpTransactions { get; set; } = new List<TopUpTransaction>();

}