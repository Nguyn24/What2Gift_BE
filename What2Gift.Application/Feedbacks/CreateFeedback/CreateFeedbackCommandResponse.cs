namespace What2Gift.Application.Feedbacks.CreateFeedback;

public sealed record CreateFeedbackCommandResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public int Rating { get; init; }
    public string? Comment { get; init; }
    public DateTime CreatedAt { get; init; }
}
