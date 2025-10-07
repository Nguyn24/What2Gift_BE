using MediatR;
using Microsoft.AspNetCore.Mvc;
using What2Gift.Apis.Extensions;
using What2Gift.Apis.Requests;
using What2Gift.Application.Abstraction.Authentication;
using What2Gift.Application.Products.CreateProduct;
using What2Gift.Application.Products.DeleteProduct;
using What2Gift.Application.Products.GetAllProduct;
using What2Gift.Application.Products.SearchProducts.SearchProduct;
using What2Gift.Application.Products.UpdateProduct;
using What2Gift.Application.Users.GetAllUser;
using What2Gift.Domain.Common;

namespace What2Gift.Apis.Controller;

[Route("api/product/")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IImageUploader _imageUploader;
    
    public ProductController(ISender mediator, IImageUploader imageUploader)
    {
        _mediator = mediator;
        _imageUploader = imageUploader;

    }

    [HttpPost("create-product")]
    public async Task<IResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand
        {
            BrandId = request.BrandId,
            CategoryId = request.CategoryId,
            OccasionId = request.OccasionId,
            Name = request.Name,
            Description = request.Description,
            ImageUrl = request.Image,
            ProductSources = request.ProductSources
                .Select(src => new CreateProductSourcesRequest
                {
                    VendorName = src.VendorName,
                    Price = src.Price,
                    AffiliateLink = src.AffiliateLink
                })
                .ToList()

        };

        Result result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();;
    }
    
    // [Authorize(Roles = "Staff")]
    [HttpGet("get-all-product")]
    public async Task<IResult> GetProduct([FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken cancellation)
    {
        Result<Page<GetProductResponse>> result = await _mediator.Send(new GetProductQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
        }, cancellation);
        return result.MatchOk();
    }
    
    // [Authorize(Roles = "Staff")]
    [HttpGet("search-product")]
    public async Task<IResult> SearchProduct(
        [FromQuery] Guid? brandId,
        [FromQuery] Guid? categoryId,
        [FromQuery] Guid? occasionId,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        CancellationToken cancellation)
    {
        Result<Page<GetProductResponse>> result = await _mediator.Send(new SearchProductQuery
        {
            BrandId = brandId,
            CategoryId = categoryId,
            OccasionId = occasionId,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            PageNumber = pageNumber,
            PageSize = pageSize,
        }, cancellation);
        return result.MatchOk();
    }
    
    [HttpPut("update-product")]
    public async Task<IResult> UpdateProduct([FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand
        {
            Id = request.Id,
            BrandId = request.BrandId,
            CategoryId = request.CategoryId,
            OccasionId = request.OccasionId,
            Name = request.Name,
            Description = request.Description,
            ImageUrl = request.Image, 
            ProductSources = request.ProductSources
        };

        Result result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }
    
    [HttpDelete("delete-product")]
    public async Task<IResult> DeleteProduct([FromBody] DeleteProductRequest request, CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand
        {
            Id = request.ProductId
        };

        Result result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }
}