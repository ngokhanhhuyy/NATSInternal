using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Application.UseCases.Photos;

namespace NATSInternal.Api.Controllers;

[Route("api/photos")]
[ApiController]
[Authorize]
public class PhotoController : ControllerBase
{
    #region Fields
    private readonly IMediator _mediator;
    #endregion

    #region Constructors
    public PhotoController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    #region Methods
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSingleById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        PhotoGetSingleRequestDto requestDto = new() { Id = id };
        return Ok(await _mediator.Send(requestDto, cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> GetMultipleByProductIds(
        [FromQuery] PhotoGetMultipleByProductIdsRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(requestDto, cancellationToken));
    }
    #endregion
}
