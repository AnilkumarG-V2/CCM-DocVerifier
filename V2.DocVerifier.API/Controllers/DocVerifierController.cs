using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using V2.DocVerifier.Models;
using V2.DocVerifier.Services;

namespace V2.DocVerifier.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocVerifierController : ControllerBase
    {

        private readonly ILogger<DocVerifierController> _logger;
        private readonly IDocVerifier _docVerifier;

        public DocVerifierController(ILogger<DocVerifierController> logger, IDocVerifier docVerifier)
        {
            _logger = logger;
            _docVerifier = docVerifier;
        }

        [HttpPost]
        public async Task<IActionResult> Post(GeminiRequest model)
        {
            GeminiViewModel viewModel = new GeminiViewModel(model);
            return new OkObjectResult(await _docVerifier.Process(viewModel));
        }
    }
}
