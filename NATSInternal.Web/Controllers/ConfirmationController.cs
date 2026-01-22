using Microsoft.AspNetCore.Mvc;
using NATSInternal.Web.Models;

namespace NATSInternal.Web.Controllers;

public class ConfirmationController : Controller
{
    #region Methods
    [HttpGet("/404")]
    public IActionResult NotFoundError()
    {
        return View("~/Views/Confirmation/NotFoundErrorPage.cshtml");
    }

    [HttpGet("/loi-du-lieu")]
    public IActionResult ValidationError([FromQuery] ConfirmationModel model)
    {
        return View("~/Views/Confirmation/ValidationErrorPage.cshtml", model);
    }

    [HttpGet("/luu-thanh-cong")]
    public IActionResult SuccessfulSubmissionPage([FromQuery] ConfirmationModel model)
    {
        return View("~/Views/Confirmation/ValidationErrorPage.cshtml", model);
    }
    #endregion
}