using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FurniComply.Web.Filters;

public class TransactionFeedbackFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var executed = await next();

        if (executed.Controller is not Controller controller)
        {
            return;
        }

        if (controller.TempData.ContainsKey("SuccessMessage") ||
            controller.TempData.ContainsKey("ErrorMessage") ||
            controller.TempData.ContainsKey("InfoMessage") ||
            controller.TempData.ContainsKey("WarningMessage"))
        {
            return;
        }

        var controllerName = context.RouteData.Values["controller"]?.ToString();
        var actionName = context.RouteData.Values["action"]?.ToString();
        if (string.Equals(controllerName, "Account", System.StringComparison.OrdinalIgnoreCase) &&
            (string.Equals(actionName, "Login", System.StringComparison.OrdinalIgnoreCase) ||
             string.Equals(actionName, "Logout", System.StringComparison.OrdinalIgnoreCase)))
        {
            return;
        }

        var isPost = HttpMethods.IsPost(context.HttpContext.Request.Method);
        if (!isPost)
        {
            return;
        }

        if (executed.Exception != null && !executed.ExceptionHandled)
        {
            controller.TempData["ErrorMessage"] = "Transaction failed. Try again.";
            return;
        }

        if (executed.Result is ViewResult && !controller.ViewData.ModelState.IsValid)
        {
            var hasModelLevelError =
                controller.ViewData.ModelState.TryGetValue(string.Empty, out var modelEntry) &&
                modelEntry.Errors.Count > 0;

            if (!hasModelLevelError)
            {
                controller.TempData["ErrorMessage"] = "Please review highlighted fields and submit again.";
            }

            return;
        }

        if (executed.Result is RedirectResult ||
            executed.Result is RedirectToRouteResult ||
            executed.Result is RedirectToActionResult ||
            executed.Result is LocalRedirectResult)
        {
            controller.TempData["SuccessMessage"] = "Transaction completed.";
        }
    }
}
