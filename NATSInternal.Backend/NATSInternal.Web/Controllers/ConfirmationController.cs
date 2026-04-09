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
    public IActionResult ValidationError()
    {
        return View("~/Views/Confirmation/ValidationErrorPage.cshtml");
    }

    [HttpGet("/luu-thanh-cong")]
    public IActionResult SuccessfulSubmissionPage()
    {
        return View("~/Views/Confirmation/ValidationErrorPage.cshtml");
    }

    [HttpGet("/xac-nhan-xoa")]
    public IActionResult DeleteConfirmation([FromQuery] DeleteConfirmationModel model)
    {
        return View("~/Views/Confirmation/DeleteConfirmationPage.cshtml", model);
    }
    #endregion
}