using What2Gift.Application.Abstraction.Messaging;

namespace What2Gift.Application.Products.DeleteProduct;

public class DeleteProductCommand : ICommand
{
    public Guid Id { get; set; } 

}