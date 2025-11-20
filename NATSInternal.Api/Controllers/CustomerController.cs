using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Application.UseCases.Customers;

namespace NATSInternal.Api.Controllers;

[Route("api/customers")]
[ApiController]
[Authorize]
public class CustomerController : ControllerBase
{
    #region Fields
    private readonly IMediator _mediator;
    #endregion

    #region Constructors
    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion
    
    #region Methods
    [HttpGet]
    [ProducesResponseType<CustomerGetListResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetList(
        [FromQuery] CustomerGetListRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(requestDto, cancellationToken));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<CustomerGetListResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDetail([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        CustomerGetDetailRequestDto requestDto = new() { Id = id };
        return Ok(await _mediator.Send(requestDto, cancellationToken));
    }
    #endregion
}