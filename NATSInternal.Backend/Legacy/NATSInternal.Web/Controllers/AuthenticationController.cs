using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Web.Extensions;
using NATSInternal.Web.Models;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.UseCases.Users;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace NATSInternal.Web.Controllers;

public class AuthenticationController : Controller
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
    [HttpGet("dang-nhap")]
    public IActionResult SignIn()
    {
        if (User.Identity is { IsAuthenticated: true })
        {
            return RedirectToAction("Index", "Dashboard");
        }

        SignInModel model = new();
        return View("~/Views/Authentication/SignIn.cshtml", model);
    }

    [AllowAnonymous]
    [HttpPost("dang-nhap")]
    public async Task<IActionResult> SignIn(SignInModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            await _mediator.Send(model.ToRequestDto(), cancellationToken);
            UserGetDetailByUserNameRequestDto userRequestDto = new()
            {
                UserName = model.UserName,
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

            ClaimsIdentity claimsIdentity = new(claims, "MvcCookie");
            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new()
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            });

            return RedirectToAction("Index", "Dashboard");
        }
        catch (Exception exception)
        {
            switch (exception)
            {
                case ValidationException validationException:
                    ModelState.AddModelErrors(validationException);
                    break;
                case OperationException operationException:
                    ModelState.AddModelErrors(operationException);
                    break;
                default:
                    throw;
            }

            return View(model);
        }
    }

    [Authorize]
    [HttpPost("dang-xuat")]
    public new async Task<IActionResult> SignOut()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("SignIn");
    }
    #endregion
}