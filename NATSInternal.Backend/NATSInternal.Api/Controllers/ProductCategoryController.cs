using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Application.UseCases.Products;
using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Api.Controllers;

[Route("api/products/categories")]
[ApiController]
[Authorize]
public class ProductCategoryController : ControllerBase
{
    #region Fields
    private readonly IMediator _mediator;
    #endregion

    #region Constructors
    public ProductCategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    [HttpGet]
    [ProducesResponseType<ProductCategoryGetListResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetList(
        [FromQuery] ProductCategoryGetListRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(requestDto, cancellationToken));
    }

    [HttpGet("all")]
    [ProducesResponseType<IEnumerable<ProductCategoryBasicResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new ProductCategoryGetAllRequestDto(), cancellationToken));
    }
}