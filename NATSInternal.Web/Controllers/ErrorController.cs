using Microsoft.AspNetCore.Mvc;
using NATSInternal.Web.Models;

namespace NATSInternal.Web.Controllers;

[Route("/loi")]
public class ErrorController : Controller
{
    #region Methods
    public IActionResult Index([FromQuery] ErrorModel model)
    {
        return View("~/Views/Error/ValidationErrorPage.cshtml", model);
    }
    #endregion
}