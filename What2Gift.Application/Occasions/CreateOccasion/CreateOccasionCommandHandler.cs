using What2Gift.Application.Abstraction.Data;
using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;
using What2Gift.Domain.Products;

namespace What2Gift.Application.Occasions.CreateOccasion;

public class CreateOccasionCommandHandler(IDbContext context) : ICommandHandler<CreateOccasionCommand>
{
    public async Task<Result> Handle(CreateOccasionCommand request, CancellationToken cancellationToken)
    {
        var occasion = new Occasion
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            StartMonth = request.StartMonth,
            StartDay = request.StartDay,
            EndMonth = request.EndMonth,
            EndDay = request.EndDay
        };

        context.Occasions.Add(occasion);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(occasion.Id);
    }
}
