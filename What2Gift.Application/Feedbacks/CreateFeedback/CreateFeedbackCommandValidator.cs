using FluentValidation;

namespace What2Gift.Application.Feedbacks.CreateFeedback;

public sealed class CreateFeedbackCommandValidator : AbstractValidator<CreateFeedbackCommand>
{
    public CreateFeedbackCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Rating).InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");
        RuleFor(x => x.Comment).MaximumLength(1000).WithMessage("Comment cannot exceed 1000 characters");
    }
}
