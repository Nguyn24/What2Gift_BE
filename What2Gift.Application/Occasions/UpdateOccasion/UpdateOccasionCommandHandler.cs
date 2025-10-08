using Microsoft.EntityFrameworkCore;
using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Products;

namespace What2Gift.Application.Occasions.UpdateOccasion;

public class UpdateOccasionCommandHandler(IDbContext context) : ICommandHandler<UpdateOccasionCommand>
{
    public async Task<Result> Handle(UpdateOccasionCommand request, CancellationToken cancellationToken)
    {
        var occasion = await context.Occasions
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

        if (occasion is null)
        {
            return Result.Failure(Error.NotFound("Occasion.NotFound", "Occasion not found"));
        }

        occasion.Name = request.Name;
        occasion.StartMonth = request.StartMonth;
        occasion.StartDay = request.StartDay;
        occasion.EndMonth = request.EndMonth;
        occasion.EndDay = request.EndDay;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
