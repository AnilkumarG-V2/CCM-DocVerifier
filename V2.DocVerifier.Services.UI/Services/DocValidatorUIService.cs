using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using V2.DocVerifier.Models;
using V2.DocVerifier.Services.UI.Interfaces;

namespace V2.DocVerifier.Services.UI.Services
{
    public class DocValidatorUIService : IDocValidator
    {
        private readonly ILogger<DocValidatorUIService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;

        public DocValidatorUIService(ILogger<DocValidatorUIService> logger, HttpClient httpClient, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<List<GeminiDestinationData>> ValidateAsync(GeminiRequest model)
        {
            var _getSourceData = await GetSourceDataAsync();
            var _geminiData = await GetGeminiDataAsync(model);
            await AssignSourceDataAsync(_geminiData, _getSourceData);
            return _geminiData;
        }

        private async Task AssignSourceDataAsync(List<GeminiDestinationData> destData, PayStubSourceData sourceData)
        {
            foreach (var data in destData)
            {
                data.Elements.Employer_City.SourceValue = sourceData.Employer_City;
                data.Elements.Employer_Name.SourceValue = sourceData.Employer_Name;
                data.Elements.Employer_Zip_Code.SourceValue = sourceData.Employer_Zip_Code;
                data.Elements.Employee_Social_Security_Number.SourceValue = sourceData.Employee_Social_Security_Number;
                data.Elements.Employer_Identification_Number.SourceValue = sourceData.Employer_Identification_Number;
            }
        }

        private async Task<List<GeminiDestinationData>> GetGeminiDataAsync(GeminiRequest model)
        {
            HttpResponseMessage _response = null;
            try
            {
                var content = new MultipartFormDataContent();
                StreamContent _fileContent = new StreamContent(model.FormFile.OpenReadStream());
                _fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.FormFile.ContentType);
                content.Add(_fileContent, model.FormFile.Name, model.FormFile.FileName);
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = model.FormFile.Name, FileName = model.FormFile.FileName };
                _response = await _httpClient.PostAsync(@$"{_configuration["DocValidatorURL"]}", content);
                var _responseData = await _response.Content.ReadAsStringAsync();
                List<GeminiDestinationData> _responseObject = JsonConvert.DeserializeObject<List<GeminiDestinationData>>(_responseData.ToString());
                return _responseObject;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<PayStubSourceData> GetSourceDataAsync()
        {
            PayStubSourceData _data = new PayStubSourceData();
            var _path = @$"{_hostEnvironment.ContentRootPath}/wwwroot/js/validator_source/validSourceData.json";
            var _jsonContent = File.ReadAllText(_path);
            PayStubSourceData _responseObject = JsonConvert.DeserializeObject<PayStubSourceData>(_jsonContent);
            return _responseObject;
        }
    }
}
