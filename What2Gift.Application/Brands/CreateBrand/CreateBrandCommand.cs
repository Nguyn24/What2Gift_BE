using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Brands.CreateBrand;

public class CreateBrandCommand : ICommand
{
    public string Name { get; init; } = string.Empty;
}
