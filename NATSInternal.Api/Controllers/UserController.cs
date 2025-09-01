using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Application.UseCases.Users;

namespace NATSInternal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    #region Fields
    private readonly IMediator _mediator;
    #endregion

    #region Constructors
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    #region Methods
    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] UserGetListRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(requestDto, cancellationToken));
    }
    #endregion
}
 