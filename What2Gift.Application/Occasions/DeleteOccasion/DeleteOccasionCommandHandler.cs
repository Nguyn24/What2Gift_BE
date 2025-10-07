using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Products;

namespace What2Gift.Application.Occasions.DeleteOccasion;

public class DeleteOccasionCommandHandler(IDbContext context) : ICommandHandler<DeleteOccasionCommand>
{
    public async Task<Result> Handle(DeleteOccasionCommand request, CancellationToken cancellationToken)
    {
        var occasion = await context.Occasions
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

        if (occasion is null)
        {
            return Result.Failure(Error.NotFound("Occasion.NotFound", "Occasion not found"));
        }

        // Check if occasion has products or gift suggestions
        var hasProducts = await context.Products
            .AnyAsync(p => p.OccasionId == request.Id, cancellationToken);

        var hasGiftSuggestions = await context.GiftSuggestions
            .AnyAsync(gs => gs.OccasionId == request.Id, cancellationToken);

        if (hasProducts || hasGiftSuggestions)
        {
            return Result.Failure(Error.Problem("Occasion.HasReferences", "Cannot delete occasion that has products or gift suggestions"));
        }

        context.Occasions.Remove(occasion);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
