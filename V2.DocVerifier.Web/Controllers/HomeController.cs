using Microsoft.AspNetCore.Mvc;
using V2.DocVerifier.Models;
using V2.DocVerifier.Services;
using Microsoft.Data.SqlClient;

namespace V2.DocVerifier.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDocVerifier _docVerifier;

        public HomeController(ILogger<HomeController> logger, IDocVerifier docVerifier)
        {
           _logger = logger;
            _docVerifier = docVerifier;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(GeminiRequest model)
        {
            HttpContext.Session.Set<List<GeminiResponse>>("docResponse", await _docVerifier.Process(model));
            return RedirectToAction("ListDocuments");
        }

        [HttpGet]
        public async Task<IActionResult> ListDocuments()
        {
            var model = HttpContext.Session.Get<List<GeminiResponse>>("docResponse");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DocumentDetails(string docType, int pageNumber)
        {
            var responseData = HttpContext.Session.Get<List<GeminiResponse>>("docResponse");
            var model = responseData.Where(data=> data.DocumentType == docType && data.PageNumber == pageNumber.ToString()).FirstOrDefault();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}

