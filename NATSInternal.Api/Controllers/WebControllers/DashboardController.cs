using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Api.Constants;

namespace NATSInternal.Api.Controllers.WebControllers;

[Route("bang-dieu-khien")]
[Authorize(AuthenticationSchemes = AuthenticationScheme.Mvc)]
public class DashboardWebController : Controller
{
    #region Methods
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    #endregion
}