using MediatR;
using Microsoft.AspNetCore.Mvc;
using What2Gift.Apis.Extensions;
using What2Gift.Apis.Requests;
using What2Gift.Application.Brands.CreateBrand;
using What2Gift.Application.Brands.DeleteBrand;
using What2Gift.Application.Brands.GetAllBrands;
using What2Gift.Application.Brands.UpdateBrand;
using What2Gift.Domain.Common;

namespace What2Gift.Apis.Controller;

[Route("api/brand/")]
[ApiController]
public class BrandController : ControllerBase
{
    private readonly ISender _mediator;

    public BrandController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create-brand")]
    public async Task<IResult> CreateBrand([FromBody] CreateBrandRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateBrandCommand
        {
            Name = request.Name
        };

        Result result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }

    [HttpGet("get-all-brands")]
    public async Task<IResult> GetAllBrands([FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        Result<Page<BrandResponse>> result = await _mediator.Send(new GetAllBrandsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
        }, cancellationToken);
        return result.MatchOk();
    }

    [HttpPut("update-brand")]
    public async Task<IResult> UpdateBrand([FromBody] UpdateBrandRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateBrandCommand
        {
            Id = request.Id,
            Name = request.Name
        };

        Result result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }

    [HttpDelete("delete-brand")]
    public async Task<IResult> DeleteBrand([FromBody] DeleteBrandRequest request, CancellationToken cancellationToken)
    {
        var command = new DeleteBrandCommand
        {
            Id = request.Id
        };

        Result result = await _mediator.Send(command, cancellationToken);
        return result.MatchOk();
    }
}
