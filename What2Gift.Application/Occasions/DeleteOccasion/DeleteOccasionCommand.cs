using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Occasions.DeleteOccasion;

public class DeleteOccasionCommand : ICommand
{
    public Guid Id { get; init; }
}
