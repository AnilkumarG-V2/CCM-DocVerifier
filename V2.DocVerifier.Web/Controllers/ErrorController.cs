using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using V2.DocVerifier.Web.Models;

namespace V2.DocVerifier.Web.Controllers
{
    [Route("/Error")]
    public class ErrorController : Controller
    {
        [AllowAnonymous] // If you want to allow anonymous access to the error page
        public IActionResult Index()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionDetails?.Error != null)
            {
                // Log the exception (e.g., using a logger)
                //_logger.LogError(exceptionDetails.Error, "An error occurred.");

                // Return a view with error details
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorMessage = exceptionDetails.Error.Message });
            }

            // Handle cases where no exception information is available (e.g., redirect to a generic error page)
            return View("Error"); //Or a default error page
        }
    }
}
