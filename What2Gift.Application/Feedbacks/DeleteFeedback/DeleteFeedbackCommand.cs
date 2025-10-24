using What2Gift.Application.Abstraction.Messaging;

namespace What2Gift.Application.Feedbacks.DeleteFeedback;

public sealed class DeleteFeedbackCommand : ICommand
{
    public Guid Id { get; init; }
}
