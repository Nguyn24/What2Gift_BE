using What2Gift.Application.Abstraction.Messaging;
using What2Gift.Domain.Common;

namespace What2Gift.Application.Brands.DeleteBrand;

public class DeleteBrandCommand : ICommand
{
    public Guid Id { get; init; }
}
