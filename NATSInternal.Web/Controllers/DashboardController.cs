using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NATSInternal.Web.Controllers;

[Route("/")]
[Authorize]
public class DashboardController : Controller
{
    #region Methods
    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Views/Dashboard/Dashboard.cshtml");
    }
    #endregion
}