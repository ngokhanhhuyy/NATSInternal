using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using IAuthenticationService = NATSInternal.Core.Interfaces.Services.IAuthenticationService;

namespace NATSInternal.Controllers;

[Route("/Api/Authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    #region Fields
    private readonly IAuthenticationService _authenticationService;
    #endregion

    #region Constructors
    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    #endregion

    #region Methods
    [HttpPost("GetAccessCookie")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> GetAccessCookie(
            SignInRequestDto requestDto,
            CancellationToken cancellationToken)
    {
        UserDetailResponseDto responseDto = await _authenticationService
            .VerifyUserNameAndPasswordAsync(requestDto, cancellationToken);

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, responseDto.Id.ToString()),
            new Claim(ClaimTypes.Name, responseDto.UserName)
        };

        IEnumerable<string> permissionNames = responseDto.Roles.SelectMany(r => r.PermissionNames);
        claims.AddRange(permissionNames.Select(pn => new Claim("Permission", pn)));
            
        ClaimsIdentity identity = new(claims, IdentityConstants.ApplicationScheme);
        ClaimsPrincipal principal = new(identity);
        AuthenticationProperties properties = new()
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
        };

        await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal, properties);

        return Ok();
    }

    [HttpPost("ClearAccessCookie")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ClearAccessCookie()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }

    [HttpGet("CheckAuthenticationStatus")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult CheckAuthenticationStatus()
    {
        return Ok();
    }
    #endregion
}
