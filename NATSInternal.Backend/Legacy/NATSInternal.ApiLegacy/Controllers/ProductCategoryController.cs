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

    [HttpGet("{id:guid}")]
    [ProducesResponseType<ProductCategoryGetDetailResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> GetDetail([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new ProductCategoryGetDetailRequestDto { Id = id }, cancellationToken));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] ProductCategoryUpdateRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        requestDto.Id = id;
        await _mediator.Send(requestDto, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        ProductCategoryDeleteRequestDto requestDto = new() { Id = id };
        await _mediator.Send(requestDto, cancellationToken);
        return Ok();
    }
}