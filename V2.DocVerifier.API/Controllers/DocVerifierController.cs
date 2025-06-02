using Microsoft.AspNetCore.Mvc;
using V2.DocVerifier.Models;
using V2.DocVerifier.Services;

namespace V2.DocVerifier.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DocVerifierController : ControllerBase
    {

        private readonly ILogger<DocVerifierController> _logger;
        private readonly IDocVerifier _docVerifier;
        private readonly IDocValidator _docValidator;

        public DocVerifierController(ILogger<DocVerifierController> logger, IDocVerifier docVerifier, IDocValidator docValidator)
        {
            _logger = logger;
            _docVerifier = docVerifier;
            _docValidator = docValidator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(GeminiRequest model)
        {
            GeminiViewModel viewModel = new GeminiViewModel(model);
            return new OkObjectResult(await _docVerifier.ProcessAsync(viewModel));
        }


        [HttpPost]
        public async Task<IActionResult> Validate(GeminiRequest model)
        {
            GeminiViewModel viewModel = new GeminiViewModel(model);
            return new OkObjectResult(await _docValidator.ValidateAsync(viewModel));
        }
    }
}
