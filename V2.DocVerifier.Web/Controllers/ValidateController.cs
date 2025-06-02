using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using V2.DocVerifier.Models;
using V2.DocVerifier.Services.UI.Interfaces;

namespace V2.DocVerifier.Web.Controllers
{
    public class ValidateController : Controller
    {
        private readonly ILogger<ValidateController> _logger;
        private readonly IDocValidator _docValidator;

        public ValidateController(ILogger<ValidateController> logger, IDocValidator docValidator)
        {
           _logger = logger;
            _docValidator = docValidator;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(GeminiRequest model)
        {
            HttpContext.Session.Set<List<GeminiDestinationData>>("docValidateResponse", await _docValidator.ValidateAsync(model));
            return RedirectToAction("ListDocuments");
        }

        [HttpGet]
        public async Task<IActionResult> ListDocuments()
        {
            var model = HttpContext.Session.Get<List<GeminiDestinationData>>("docValidateResponse");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DocumentDetails(string docType, int pageNumber)
        {
            var responseData = HttpContext.Session.Get<List<GeminiDestinationData>>("docValidateResponse");
            var model = responseData.Where(data=> data.DocumentType == docType && data.PageNumber == pageNumber.ToString()).FirstOrDefault();
            return View(model);
        }
    }
}

