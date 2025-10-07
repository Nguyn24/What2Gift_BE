using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Categories.CreateCategory;

public class CreateCategoryCommand : ICommand
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}
