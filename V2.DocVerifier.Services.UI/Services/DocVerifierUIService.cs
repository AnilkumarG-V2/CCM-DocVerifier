using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using V2.DocVerifier.Models;
using V2.DocVerifier.Services.UI.Interfaces;

namespace V2.DocVerifier.Services.UI.Services
{
    public class DocVerifierUIService : IDocVerifier
    {
        private readonly ILogger<DocVerifierUIService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;

        public DocVerifierUIService(ILogger<DocVerifierUIService> logger, HttpClient httpClient, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// Method for uploading the files for extraction of data using Gemini.
        /// </summary>
        /// <param name="model">GeminiRequest</param>
        /// <returns>List<GeminiResponse></returns>
        public async Task<List<GeminiResponse>> ExecuteAsync(GeminiRequest model)
        {
            HttpResponseMessage _response = null;
            try
            {
                var content = new MultipartFormDataContent();
                StreamContent _fileContent = new StreamContent(model.FormFile.OpenReadStream());
                _fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.FormFile.ContentType);
                content.Add(_fileContent, model.FormFile.Name, model.FormFile.FileName);
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = model.FormFile.Name, FileName = model.FormFile.FileName };
                _response = await _httpClient.PostAsync(@$"{_configuration["DocVerifierURL"]}", content);
                var _responseData = await _response.Content.ReadAsStringAsync();
                List<GeminiResponse> _responseObject =  JsonConvert.DeserializeObject<List<GeminiResponse>>(_responseData.ToString());
                _fileContent.Dispose();
                return _responseObject;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get the list of result for the 37 paystubs files
        /// </summary>
        /// <returns>List<GeminiPayStubResult></returns>
        public async Task<List<GeminiPayStubResult>> GetPayStubResultsAsync()
        {
            List<GeminiPayStubResult> _collection = new List<GeminiPayStubResult>();
            var _path = @$"{_hostEnvironment.ContentRootPath}/wwwroot/js/results";
            var _jsonResults = Directory.GetFiles(_path);

            foreach(var _jsonResult in _jsonResults)
            {
                _collection.Add(new GeminiPayStubResult() { Name = Path.GetFileNameWithoutExtension(_jsonResult), FileName = Path.GetFileName(_jsonResult), Type = "Paystub" });
            }

            return _collection;
        }

        /// <summary>
        /// Get the results details for the 37 paystubs files
        /// </summary>
        /// <param name="fileName">string</param>
        /// <returns>List<GeminiResponse></returns>
        public async Task<List<GeminiResponse>> LoadPayStubAsync(string fileName)
        {
            var _path = @$"{_hostEnvironment.ContentRootPath}/wwwroot/js/results/{fileName}";
            var _jsonContent = File.ReadAllText(_path);
            List<GeminiResponse> _responseObject = JsonConvert.DeserializeObject<List<GeminiResponse>>(_jsonContent);
            return _responseObject;
        }
    }
}
