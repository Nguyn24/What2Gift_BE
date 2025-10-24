using What2Gift.Application.Abstraction.Messaging;

namespace What2Gift.Application.Feedbacks.CreateFeedback;

public sealed class CreateFeedbackCommand : ICommand<CreateFeedbackCommandResponse>
{
    public Guid UserId { get; init; }
    public int Rating { get; init; }
    public string? Comment { get; init; }
}
