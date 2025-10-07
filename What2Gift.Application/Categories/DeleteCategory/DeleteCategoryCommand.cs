using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Categories.DeleteCategory;

public class DeleteCategoryCommand : ICommand
{
    public Guid Id { get; init; }
}
