using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using V2.DocVerifier.Models;
using V2.DocVerifier.Services.UI.Interfaces;

namespace V2.DocVerifier.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDocVerifier _docVerifier;

        public HomeController(IDocVerifier docVerifier)
        {
            _docVerifier = docVerifier;
        }

        /// <summary>
        /// UI for uploading the file for extraction process.
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Action method to extract the data then redirect the user to the listing page.
        /// </summary>
        /// <param name="model">GeminiRequest</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Index(GeminiRequest model)
        {
            HttpContext.Session.Set<List<GeminiResponse>>("docResponse", await _docVerifier.ExecuteAsync(model));
            return RedirectToAction("ListDocuments");
        }

        /// <summary>
        /// Listing the document extracted data based on the page.
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public async Task<IActionResult> ListDocuments()
        {
            var model = HttpContext.Session.Get<List<GeminiResponse>>("docResponse");
            return View(model);
        }

        /// <summary>
        /// Action method to display the extracted data on the screen.
        /// </summary>
        /// <param name="docType">string</param>
        /// <param name="pageNumber">int</param>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public async Task<IActionResult> DocumentDetails(string docType, int pageNumber)
        {
            var responseData = HttpContext.Session.Get<List<GeminiResponse>>("docResponse");
            var model = responseData.Where(data=> data.DocumentType == docType && data.PageNumber == pageNumber.ToString()).FirstOrDefault();
            return View(model);
        }

        /// <summary>
        /// List the 37 pay stub data.
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public async Task<IActionResult> ListPayStubResults()
        {
            var model = await _docVerifier.GetPayStubResultsAsync();
            return View(model);
        }

        /// <summary>
        /// Load the paystub results
        /// </summary>
        /// <param name="fileName">string</param>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public async Task<IActionResult> LoadPayStub(string fileName)
        {
            HttpContext.Session.Set<List<GeminiResponse>>("docResponse", await _docVerifier.LoadPayStubAsync(fileName));
            return RedirectToAction("ListDocuments");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}

