using What2Gift.Domain.Common;
using What2Gift.Domain.Users;

namespace What2Gift.Domain.Products;

public class GiftSuggestion : Entity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? OccasionId { get; set; }
    public RecipientGender RecipientGender { get; set; } 
    public int RecipientAge { get; set; }
    public string RecipientHobby { get; set; } = null!;
    public decimal BudgetMin { get; set; }
    public decimal BudgetMax { get; set; }
    public Guid? SuggestedProductId { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
    public Occasion? Occasion { get; set; }
    public Product? SuggestedProduct { get; set; }
    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}