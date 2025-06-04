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

        public async Task<List<GeminiDestinationData>> ValidateAsync(GeminiViewModel model)
        {
            await _fileProcessor.SplitFileAsync(model);
            model.FileName = model.FormFile.FileName;
            var json = await ProcessTaskAsync(model);
            return json;
        }

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
            t.Wait();
            return response;
        }

        private async Task ProcessFilesAsync(GeminiViewModel model, int pageNumber, List<GeminiDestinationData> response, string file)
        {
            var _responseData = await _geminiClient.GetGeminiResponseAsync<GeminiDestinationData>(Resource.ValidatorPrompt, file);
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
