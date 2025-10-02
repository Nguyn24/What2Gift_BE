using What2Gift.Domain.Common;

namespace What2Gift.Domain.Products.Errors;

public class ProductErrors
{
    public static readonly Error BrandNotFound = Error.NotFound(
        "Product.BrandNotFound",
        "The specified brand does not exist.");

    public static readonly Error CategoryNotFound = Error.NotFound(
        "Product.CategoryNotFound",
        "The specified category does not exist.");

    public static readonly Error OccasionNotFound = Error.NotFound(
        "Product.OccasionNotFound",
        "The specified occasion does not exist.");

    public static readonly Error InvalidProductSource = Error.Problem(
        "Product.InvalidProductSource",
        "One or more product sources are invalid.");

    public static readonly Error NameRequired = Error.Problem(
        "Product.NameRequired",
        "Product name is required.");
    
    public static readonly Error ProductNotFound = Error.NotFound(
        "Product.NotFound",
        "The product does not exist.");
}