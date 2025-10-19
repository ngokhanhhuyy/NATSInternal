using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Application.UseCases.Authentication;
using NATSInternal.Application.UseCases.Users;

namespace NATSInternal.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    #region Fields
    private readonly IMediator _mediator;
    #endregion

    #region Constructors
    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    #region Methods
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> GetAccessCookie(
        VerifyUserNameAndPasswordRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        await _mediator.Send(requestDto, cancellationToken);

        UserGetDetailByUserNameRequestDto userRequestDto = new()
        {
            UserName = requestDto.UserName,
            IncludingAuthorization = false
        };
        
        UserGetDetailResponseDto userResponseDto = await _mediator.Send(userRequestDto, cancellationToken);

        List<Claim> claims = new()
        {
            new(ClaimTypes.NameIdentifier, userResponseDto.Id.ToString()),
            new(ClaimTypes.Name, userResponseDto.UserName),
            new("PowerLevel", userResponseDto.Roles.Max(r => r.PowerLevel).ToString())
        };

        foreach (UserGetDetailRoleResponseDto roleResponseDto in userResponseDto.Roles)
        {
            claims.Add(new(ClaimTypes.Role, roleResponseDto.Name));
            claims.AddRange(roleResponseDto.PermissionNames.Select(pn => new Claim("Permission", pn)));
        }

        ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal,
            new()
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            });

        return Ok();
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ClearAccessCookieAsync()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }

    [Authorize]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        await _mediator.Send(requestDto, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult CheckAuthenticationStatus()
    {
        return Ok();
    }
    #endregion
}
