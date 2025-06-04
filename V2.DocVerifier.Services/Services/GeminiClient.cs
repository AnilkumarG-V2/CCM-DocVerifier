using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using V2.DocVerifier.Models;
using V2.DocVerifier.Services.Interfaces;
using V2.DocVerifier.Services.Resources;

namespace V2.DocVerifier.Services.Services
{
    public class GeminiClient : IGeminiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GeminiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<T>> GetGeminiResponseAsync<T>(string prompt, string fileName) where T : GeminiBaseModel
        {
            try
            {
                HttpResponseMessage _response = null;
                string fileContent = string.Empty;
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    var fileBytes = System.IO.File.ReadAllBytes(fileName);
                    fileContent = Convert.ToBase64String(fileBytes);
                    var jsonFile = GetPrompt(prompt, fileContent);
                    HttpContent content = new StringContent(jsonFile, Encoding.UTF8, Resource.ResponseMimeType);
                    _response = await _httpClient.PostAsync(@$"{_configuration[Resource.DocVerifierEndPoint]}{_configuration[Resource.APIKey]}", content);
                }
                var _responseData = await _response.Content.ReadAsStringAsync();
                var _json = JsonConvert.DeserializeObject<dynamic>(_responseData);
                var _text = _json?.candidates[0]?.content?.parts[0].text;
                List<T> _responseObject = JsonConvert.DeserializeObject<List<T>>(_text.ToString());
                _responseObject.ForEach(x => x.ImageContent = fileContent);
                return _responseObject;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string GetPrompt(string prompt, string fileContent)
        {
            var _geminiPrompt = new GeminiPrompt();
            var _content = new Content();
            _content.Parts.Add(new { text = prompt });
            _content.Parts.Add(new { inline_data = new InlineData() { Data = fileContent } });
            _geminiPrompt.Contents.Add(_content);
            return JsonConvert.SerializeObject(_geminiPrompt);
        }
    }
}
