using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Authentication;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Users;
using What2Gift.Domain.Users.Errors;

namespace What2Gift.Application.Feedbacks.CreateFeedback;

public sealed class CreateFeedbackCommandHandler(IDbContext context)
    : ICommandHandler<CreateFeedbackCommand, CreateFeedbackCommandResponse>
{
    public async Task<Result<CreateFeedbackCommandResponse>> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
    {
        // Verify user exists
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            return Result.Failure<CreateFeedbackCommandResponse>(UserErrors.NotFound(request.UserId));
        }

        var feedback = new Feedback
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Rating = request.Rating,
            Comment = request.Comment,
            CreatedAt = DateTime.UtcNow
        };

        context.Feedbacks.Add(feedback);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateFeedbackCommandResponse
        {
            Id = feedback.Id,
            UserId = feedback.UserId,
            Rating = feedback.Rating,
            Comment = feedback.Comment,
            CreatedAt = feedback.CreatedAt
        };
    }
}
