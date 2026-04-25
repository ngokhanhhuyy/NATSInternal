// using MediatR;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using NATSInternal.Application.UseCases.Products;
// using NATSInternal.Application.UseCases.Shared;

// namespace NATSInternal.Api.Controllers;

// [Route("api/products/brands")]
// [ApiController]
// [Authorize]
// public class BrandController : ControllerBase
// {
//     #region Fields
//     private readonly IMediator _mediator;
//     #endregion

//     #region Constructors
//     public BrandController(IMediator mediator)
//     {
//         _mediator = mediator;
//     }
//     #endregion

//     [HttpGet]
//     [ProducesResponseType<BrandGetListResponseDto>(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//     public async Task<IActionResult> GetList(
//         [FromQuery] BrandGetListRequestDto requestDto,
//         CancellationToken cancellationToken = default)
//     {
//         return Ok(await _mediator.Send(requestDto, cancellationToken));
//     }

//     [HttpGet("all")]
//     [ProducesResponseType<IEnumerable<BrandBasicResponseDto>>(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//     public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
//     {
//         return Ok(await _mediator.Send(new BrandGetAllRequestDto(), cancellationToken));
//     }

//     [HttpGet("{id:guid}")]
//     [ProducesResponseType<BrandGetDetailResponseDto>(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status409Conflict)]
//     public async Task<IActionResult> GetDetail([FromRoute] Guid id, CancellationToken cancellationToken = default)
//     {
//         return Ok(await _mediator.Send(new BrandGetDetailRequestDto { Id = id }, cancellationToken));
//     }

//     [HttpPost]
//     [ProducesResponseType<Guid>(StatusCodes.Status201Created)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//     [ProducesResponseType(StatusCodes.Status409Conflict)]
//     public async Task<IActionResult> Create(
//         [FromBody] BrandCreateRequestDto requestDto,
//         CancellationToken cancellationToken = default)
//     {
//         Guid id = await _mediator.Send(requestDto, cancellationToken);
//         return CreatedAtAction(nameof(GetDetail), new { id }, id);
//     }

//     [HttpPut("{id:guid}")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status409Conflict)]
//     public async Task<IActionResult> Update(
//         [FromRoute] Guid id,
//         [FromBody] BrandUpdateRequestDto requestDto,
//         CancellationToken cancellationToken = default)
//     {
//         requestDto.Id = id;
//         await _mediator.Send(requestDto, cancellationToken);
//         return Ok();
//     }

//     [HttpDelete("{id:guid}")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status409Conflict)]
//     public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
//     {
//         BrandDeleteRequestDto requestDto = new() { Id = id };
//         await _mediator.Send(requestDto, cancellationToken);
//         return Ok();
//     }
// }