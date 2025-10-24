using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Users;
using What2Gift.Domain.Users.Errors;

namespace What2Gift.Application.Feedbacks.UpdateFeedback;

public sealed class UpdateFeedbackCommandHandler(IDbContext context)
    : ICommandHandler<UpdateFeedbackCommand, UpdateFeedbackCommandResponse>
{
    public async Task<Result<UpdateFeedbackCommandResponse>> Handle(UpdateFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedback = await context.Feedbacks
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (feedback == null)
        {
            return Result.Failure<UpdateFeedbackCommandResponse>(FeedbackErrors.NotFound(request.Id));
        }

        feedback.Rating = request.Rating;
        feedback.Comment = request.Comment;

        await context.SaveChangesAsync(cancellationToken);

        return new UpdateFeedbackCommandResponse
        {
            Id = feedback.Id,
            UserId = feedback.UserId,
            Rating = feedback.Rating,
            Comment = feedback.Comment,
            CreatedAt = feedback.CreatedAt
        };
    }
}
