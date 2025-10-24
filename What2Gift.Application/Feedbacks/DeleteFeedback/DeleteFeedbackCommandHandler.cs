using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Users;
using What2Gift.Domain.Users.Errors;

namespace What2Gift.Application.Feedbacks.DeleteFeedback;

public sealed class DeleteFeedbackCommandHandler(IDbContext context)
    : ICommandHandler<DeleteFeedbackCommand>
{
    public async Task<Result> Handle(DeleteFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedback = await context.Feedbacks
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (feedback == null)
        {
            return Result.Failure(FeedbackErrors.NotFound(request.Id));
        }

        context.Feedbacks.Remove(feedback);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
