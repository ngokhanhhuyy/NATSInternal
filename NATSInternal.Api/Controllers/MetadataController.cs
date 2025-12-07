using MediatR;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Application.UseCases.Metadata;

namespace NATSInternal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetadataController : ControllerBase
{
    #region Fields
    private readonly IMediator _mediator;
    #endregion
    
    #region Constructors
    public MetadataController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion
    
    #region Methods
    [HttpGet]
    [ProducesResponseType<MetadataGetResponseDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        MetadataGetRequestDto requestDto = new();
        return Ok(await _mediator.Send(requestDto, cancellationToken));
    }
    #endregion
}