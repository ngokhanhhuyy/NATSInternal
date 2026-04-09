using Microsoft.AspNetCore.Mvc;
using NATSInternal.Web.Models;

namespace NATSInternal.Web.Extensions;

public static class ControllerExtensions
{
    #region ExtensionMethods
    public static IActionResult DeleteConfirmationView(this Controller controller, DeleteConfirmationModel? model)
    {
        model ??= new();
        return controller.View("~/Views/Confirmation/DeleteConfirmationPage.cshtml", model);
    }

    public static IActionResult SuccessfulSubmissionConfirmationView(this Controller controller, string? returningUrl)
    {
        SuccessfulSubmissionConfirmationModel model = new(returningUrl);
        return controller.View("~/Views/Confirmation/SuccessfulSubmissionConfirmationPage.cshtml", model);
    }

    public static IActionResult RedirectToSuccessfulSubmissionConfirmationAction(
        this Controller controller,
        string? returningUrl)
    {
        if (returningUrl is not null)
        {
            return controller.LocalRedirect(returningUrl);
        }

        return controller.RedirectToAction("Index", "Dashboard");
    }
    #endregion
}