namespace What2Gift.Application.Feedbacks.UpdateFeedback;

public sealed record UpdateFeedbackCommandResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public int Rating { get; init; }
    public string? Comment { get; init; }
    public DateTime CreatedAt { get; init; }
}
