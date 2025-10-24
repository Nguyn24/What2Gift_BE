using What2Gift.Application.Abstraction.Messaging;

namespace What2Gift.Application.Feedbacks.UpdateFeedback;

public sealed class UpdateFeedbackCommand : ICommand<UpdateFeedbackCommandResponse>
{
    public Guid Id { get; init; }
    public int Rating { get; init; }
    public string? Comment { get; init; }
}
