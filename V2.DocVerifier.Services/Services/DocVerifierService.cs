using Microsoft.Extensions.Logging;
using V2.DocVerifier.Models;
using V2.DocVerifier.Services.Interfaces;
using V2.DocVerifier.Services.Resources;

namespace V2.DocVerifier.Services
{
    public class DocVerifierService : IDocVerifier
    {
        private readonly ILogger<DocVerifierService> _logger;
        private readonly IFileProcessor _fileProcessor;
        private readonly IGeminiClient _geminiClient;

        public DocVerifierService(ILogger<DocVerifierService> logger, IFileProcessor fileProcessor, IGeminiClient geminiClient)
        {
            _logger = logger;
            _fileProcessor = fileProcessor;
            _geminiClient = geminiClient;
        }

        /// <summary>
        /// method for uploading the files for extraction of data using Gemini.
        /// </summary>
        /// <param name="model">GeminiViewModel</param>
        /// <returns>List<GeminiResponse></returns>
        public async Task<List<GeminiResponse>> ProcessAsync(GeminiViewModel model)
        {
            await _fileProcessor.SplitFileAsync(model);
            model.FileName = model.FormFile.FileName;
            var json = await ProcessTaskAsync(model);
            return json;
        }

        /// <summary>
        /// Method to process all the async task joined together
        /// </summary>
        /// <param name="model">GeminiModel</param>
        /// <returns>List<GeminiResponse></returns>
        private async Task<List<GeminiResponse>> ProcessTaskAsync(GeminiViewModel model)
        {
            var tasks = new List<Task>();
            int pageNumber = 1;
            List<GeminiResponse> response = new List<GeminiResponse>();
            foreach (var file in model.ImageFiles)
            {
                tasks.Add(ProcessFilesAsync(model, pageNumber, response, file));
            }
            Task t = Task.WhenAll(tasks);
            CancellationToken cancellationToken = default;
            await t.WaitAsync(cancellationToken);
            return response;
        }

        /// <summary>
        /// Processes the file and Gets the response from the Google Gemini.
        /// </summary>
        /// <param name="model">GeminiViewModel</param>
        /// <param name="pageNumber">int</param>
        /// <param name="response">List<GeminiResponse></param>
        /// <param name="file">string</param>
        /// <returns></returns>
        private async Task ProcessFilesAsync(GeminiViewModel model, int pageNumber, List<GeminiResponse> response, string file)
        {
            var _responseData = await _geminiClient.GetGeminiResponseAsync<GeminiResponse>(Resource.DocDataPrompt, file);
            if (_responseData != null)
            {
                foreach (var item in _responseData)
                {
                    item.PageNumber = Convert.ToString(pageNumber++);
                    item.ImageName = Path.GetFileName(file);
                    item.ContentType = model.FormFile.ContentType;
                    item.FileName = model.FileName;
                    response.Add(item);
                }
            }
        }
    }
}
