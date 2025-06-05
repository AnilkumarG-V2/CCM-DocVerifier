using Microsoft.AspNetCore.Mvc;
using V2.DocVerifier.Models;
using V2.DocVerifier.Services;

namespace V2.DocVerifier.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DocVerifierController : ControllerBase
    {
        private readonly IDocVerifier _docVerifier;
        private readonly IDocValidator _docValidator;

        public DocVerifierController(IDocVerifier docVerifier, IDocValidator docValidator)
        {
            _docVerifier = docVerifier;
            _docValidator = docValidator;
        }

        /// <summary>
        /// Action method for uploading the files for extraction of data using Gemini.
        /// </summary>
        /// <param name="model">GeminiRequest</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Post(GeminiRequest model)
        {
            GeminiViewModel viewModel = new GeminiViewModel(model);
            return new OkObjectResult(await _docVerifier.ProcessAsync(viewModel));
        }

        /// <summary>
        /// Action method for validating the data stored in json file against the extracted data using Gemini
        /// </summary>
        /// <param name="model">GeminiRequest</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Validate(GeminiRequest model)
        {
            GeminiViewModel viewModel = new GeminiViewModel(model);
            return new OkObjectResult(await _docValidator.ValidateAsync(viewModel));
        }
    }
}
