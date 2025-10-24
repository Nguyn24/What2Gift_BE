namespace What2Gift.Application.Feedbacks.GetAllFeedbacks;

public sealed record GetAllFeedbacksResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public int Rating { get; init; }
    public string? Comment { get; init; }
    public DateTime CreatedAt { get; init; }
}
