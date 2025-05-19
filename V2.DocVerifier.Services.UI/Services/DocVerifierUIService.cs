using Microsoft.Extensions.Configuration;
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

        public DocVerifierUIService(ILogger<DocVerifierUIService> logger, HttpClient httpClient, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<GeminiResponse>> Execute(GeminiRequest model)
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
                return _responseObject;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
