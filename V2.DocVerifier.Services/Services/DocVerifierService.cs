using Newtonsoft.Json;
using System.Text;
using V2.DocVerifier.Models;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using V2.DocVerifier.Services.Resources;

namespace V2.DocVerifier.Services
{
    public class DocVerifierService : IDocVerifier
    {
        private readonly ILogger<DocVerifierService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public DocVerifierService(ILogger<DocVerifierService> logger, HttpClient httpClient, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<GeminiResponse>> Process(GeminiViewModel model)
        {
            await SplitFile(model);
            model.FileName = model.FormFile.FileName;
            var json = await ProcessTask(model);
            return json;
        }

        public async Task<List<GeminiResponse>> ProcessTask(GeminiViewModel model)
        {
            var tasks = new List<Task>();
            int pageNumber = 1;
            List<GeminiResponse> response = new List<GeminiResponse>();
            foreach (var file in model.ImageFiles)
            {
                tasks.Add(ProcessFiles(model, pageNumber, response, file));
            }
            Task t = Task.WhenAll(tasks);
            t.Wait();
            return response;
        }

        private async Task ProcessFiles(GeminiViewModel model, int pageNumber, List<GeminiResponse> response, string file)
        {
            var _responseData = await GetGeminiResponse(Resource.DocDataPrompt, file);
            foreach (var item in _responseData)
            {
                item.PageNumber = Convert.ToString(pageNumber++);
                item.ImageName = Path.GetFileName(file);
                item.ContentType = model.FormFile.ContentType;
                item.FileName = model.FileName;
                item.FilePath = model.FilePath;
                response.Add(item);
            }
        }

        private async Task SplitFile(GeminiViewModel model)
        {
            string _datetime = DateTime.Now.ToString(Resource.FileNameDateTimeFormat);
            if (model.FormFile.Length > 0)
            {
                var _path = @$"{_configuration[Resource.ConfiguredImagePath]}{_datetime}";
                Directory.CreateDirectory(_path);

                model.FilePath = _path;

                string filePathWithFileName = $@"{_path}\{model.FormFile.FileName}";

                using (Stream fileStream = new FileStream(filePathWithFileName, FileMode.Create))
                {
                    await model.FormFile.CopyToAsync(fileStream);
                }

                if (model.FormFile.ContentType.ToLower().Contains(Resource.PDFFileExtension))
                {
                    PdfDocument pdfDocument = new PdfDocument();
                    pdfDocument.LoadFromFile(filePathWithFileName);

                    for (int _counter = 0; _counter < pdfDocument.Pages.Count; ++_counter)
                    {
                        var image = pdfDocument.SaveAsImage(_counter, PdfImageType.Bitmap);
                        image.Save(@$"{_path}\{Resource.ImageFilePrefix}{_counter + 1}.{Resource.DestinationImageFileExtension}");
                        model.ImageFiles.Add(@$"{_path}\{Resource.ImageFilePrefix}{_counter + 1}.{Resource.DestinationImageFileExtension}");
                    }
                }
                else
                {
                    model.ImageFiles.Add(filePathWithFileName);
                }
            }
        }

        private async Task<List<GeminiResponse>> GetGeminiResponse(string prompt, string fileName)
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
                List<GeminiResponse> _responseObject =  JsonConvert.DeserializeObject<List<GeminiResponse>>(_text.ToString());
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
