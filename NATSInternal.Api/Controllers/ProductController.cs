using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Application.UseCases.Products;

namespace NATSInternal.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController : ControllerBase
{
    #region Fields
    private readonly IMediator _mediator;
    #endregion

    #region Constructors
    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    #region Methods
    [HttpGet]
    [ProducesResponseType<ProductGetListResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetList(
        [FromQuery] ProductGetListRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(requestDto, cancellationToken));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<ProductGetDetailResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDetail(Guid id, CancellationToken cancellationToken = default)
    {
        ProductGetDetailRequestDto requestDto = new() { Id = id };
        return Ok(await _mediator.Send(requestDto, cancellationToken));
    }

    [HttpPost]
    [ProducesResponseType<Guid>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] ProductCreateRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        Guid id = await _mediator.Send(requestDto, cancellationToken);
        return CreatedAtAction(nameof(GetDetail), new { id }, id);
    }
    #endregion
}
