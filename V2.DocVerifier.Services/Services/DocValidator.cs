using V2.DocVerifier.Models;
using Microsoft.Extensions.Logging;
using V2.DocVerifier.Services.Resources;
using V2.DocVerifier.Services.Interfaces;

namespace V2.DocVerifier.Services
{
    public class DocValidatorService : IDocValidator
    {
        private readonly ILogger<DocValidatorService> _logger;
        private readonly IFileProcessor _fileProcessor;
        private readonly IGeminiClient _geminiClient;

        public DocValidatorService(ILogger<DocValidatorService> logger, IFileProcessor fileProcessor, IGeminiClient geminiClient)
        {
            _logger = logger;
            _fileProcessor = fileProcessor;
            _geminiClient = geminiClient;
        }

        /// <summary>
        /// Method for validating the data stored in json file against the extracted data using Gemini
        /// </summary>
        /// <param name="model">GeminiViewModel</param>
        /// <returns>List<GeminiDestinationData></returns>
        public async Task<List<GeminiDestinationData>> ValidateAsync(GeminiViewModel model)
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
        /// <returns>List<GeminiDestinationData></returns>
        private async Task<List<GeminiDestinationData>> ProcessTaskAsync(GeminiViewModel model)
        {
            var tasks = new List<Task>();
            int pageNumber = 1;
            List<GeminiDestinationData> response = new List<GeminiDestinationData>();
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
        /// <param name="model">GeminiModel</param>
        /// <param name="pageNumber">int</param>
        /// <param name="response">List<GeminiDestinationData></param>
        /// <param name="file">string</param>
        /// <returns></returns>
        private async Task ProcessFilesAsync(GeminiViewModel model, int pageNumber, List<GeminiDestinationData> response, string file)
        {
            var _responseData = await _geminiClient.GetGeminiResponseAsync<GeminiDestinationData>(Resource.ValidatorPrompt, file);
            if(_responseData != null)
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
