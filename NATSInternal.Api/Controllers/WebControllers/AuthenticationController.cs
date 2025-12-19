using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Api.Extensions;
using NATSInternal.Api.Models;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.UseCases.Users;
using System.Security.Claims;

namespace NATSInternal.Api.Controllers.WebControllers;

public class AuthenticationWebController : Controller
{
    #region Fields
    private readonly IMediator _mediator;
    #endregion

    #region Constructors
    public AuthenticationWebController(IMediator mediator)
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
            return RedirectToAction(nameof(DashboardWebController.Index), nameof(DashboardWebController));
        }

        return View();
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

            await HttpContext.SignInAsync("MvcCookie", claimsPrincipal, new()
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            });

            return RedirectToAction(nameof(DashboardWebController.Index), nameof(DashboardWebController));
        }
        catch (Exception exception)
        {
            if (exception is ValidationException validationException)
            {
                ModelState.AddModelErrors(validationException);
            }
            else if (exception is OperationException operationException)
            {
                ModelState.AddModelErrors(operationException);
            }
            else
            {
                throw;
            }

            return View(model);
        }
    }
    #endregion
}