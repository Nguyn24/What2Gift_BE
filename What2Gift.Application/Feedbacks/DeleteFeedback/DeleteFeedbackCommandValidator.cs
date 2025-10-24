using FluentValidation;

namespace What2Gift.Application.Feedbacks.DeleteFeedback;

public sealed class DeleteFeedbackCommandValidator : AbstractValidator<DeleteFeedbackCommand>
{
    public DeleteFeedbackCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
