using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Occasions.CreateOccasion;

public class CreateOccasionCommand : ICommand
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int StartMonth { get; init; }
    public int StartDay { get; init; }
    public int EndMonth { get; init; }
    public int EndDay { get; init; }
}
