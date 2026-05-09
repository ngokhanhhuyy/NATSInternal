using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Core.Common.Security;
using NATSInternal.Core.Features.Authentication;
using NATSInternal.Core.Features.Users;
using IAuthenticationService = NATSInternal.Core.Features.Authentication.IAuthenticationService;

namespace NATSInternal.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    #region Fields
    private readonly ICallerDetailProvider _callerDetailProvider;
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserService _userService;
    #endregion

    #region Constructors
    public AuthenticationController(
        ICallerDetailProvider callerDetailProvider,
        IAuthenticationService authenticationService,
        IUserService userService)
    {
        _callerDetailProvider = callerDetailProvider;
        _authenticationService = authenticationService;
        _userService = userService;
    }
    #endregion

    #region Methods
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> GetAccessCookie([FromBody] VerifyUserNameAndPasswordRequestDto requestDto)
    {
        Console.WriteLine(nameof(GetAccessCookie));
        await _authenticationService.VerifyUserNameAndPasswordAsync(requestDto);
        UserDetailResponseDto  userResponseDto;
        userResponseDto = await _userService.GetDetailByUserNameAsync(requestDto.UserName, false);

        List<Claim> claims = new()
        {
            new(ClaimTypes.NameIdentifier, userResponseDto.Id.ToString()),
            new(ClaimTypes.Name, userResponseDto.UserName),
            new("PowerLevel", userResponseDto.Roles.Max(r => r.PowerLevel).ToString())
        };

        foreach (RoleDetailResponseDto roleResponseDto in userResponseDto.Roles)
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
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CallerDetail()
    {
        return Ok(await _userService.GetDetailByIdAsync(_callerDetailProvider.GetId()));
    }

    [Authorize]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto requestDto)
    {
        await _authenticationService.ChangePasswordAsync(requestDto);
        return Ok();
    }
    

    [HttpPost("{id:int}/[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ResetPassword([FromRoute] int id, [FromBody] ResetPasswordRequestDto requestDto)
    {
        await _authenticationService.ResetPasswordAsync(id, requestDto);
        return Ok();
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult CheckAuthenticationStatus()
    {
        return Ok();
    }
    #endregion
}
