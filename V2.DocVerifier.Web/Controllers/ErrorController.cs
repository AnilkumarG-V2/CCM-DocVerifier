using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using V2.DocVerifier.Models;

namespace V2.DocVerifier.Web.Controllers
{
    [Route("/Error")]
    public class ErrorController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionDetails?.Error != null)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorMessage = exceptionDetails.Error.Message });
            }
            return View("Error");
        }
    }
}
