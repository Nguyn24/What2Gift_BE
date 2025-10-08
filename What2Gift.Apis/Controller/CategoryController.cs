using MediatR;
using Microsoft.AspNetCore.Mvc;
using What2Gift.Apis.Extensions;
using What2Gift.Apis.Requests;
using What2Gift.Application.Categories.CreateCategory;
using What2Gift.Application.Categories.DeleteCategory;
using What2Gift.Application.Categories.GetAllCategories;
using What2Gift.Application.Categories.UpdateCategory;
using What2Gift.Domain.Common;

namespace What2Gift.Apis.Controller;

[Route("api/category/")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ISender _mediator;

    public CategoryController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create-category")]
    public async Task<IResult> CreateCategory([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand
        {
            Name = request.Name
        };

        Result result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }

    [HttpGet("get-all-categories")]
    public async Task<IResult> GetAllCategories([FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        Result<Page<CategoryResponse>> result = await _mediator.Send(new GetAllCategoriesQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
        }, cancellationToken);
        return result.MatchOk();
    }

    [HttpPut("update-category")]
    public async Task<IResult> UpdateCategory([FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCategoryCommand
        {
            Id = request.Id,
            Name = request.Name
        };

        Result result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }

    [HttpDelete("delete-category")]
    public async Task<IResult> DeleteCategory([FromBody] DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new DeleteCategoryCommand
        {
            Id = request.Id
        };

        Result result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }
}
