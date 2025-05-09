using Newtonsoft.Json;
using System.Drawing;
using System.Text;
using V2.DocVerifier.Models;
using static System.Net.Mime.MediaTypeNames;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Image = System.Drawing.Image;
using System.Drawing.Imaging;
using Microsoft.Extensions.Configuration;
using V2.DocVerifier.Services.Resources;
using System.Runtime.CompilerServices;

namespace V2.DocVerifier.Services
{
    public class DocVerifierService : IDocVerifier
    {
        private readonly ILogger<DocVerifierService> _logger;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public DocVerifierService(ILogger<DocVerifierService> logger, IHostEnvironment hostEnvironment, HttpClient httpClient, IConfiguration configuration)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<GeminiResponse>> Process(GeminiRequest model)
        {
            await SplitFile(model);
            model.FileName = model.FormFile.FileName;
            model.ContentType = model.FormFile.ContentType;
            model.Json = await ProcessFiles(model);
            var _response = JsonConvert.DeserializeObject<List<GeminiResponse>>(model.Json);
            return _response;
        }

        private async Task SplitFile(GeminiRequest model)
        {
            string _datetime = DateTime.Now.ToString(Resource.FileNameDateTimeFormat);
            if (model.FormFile.Length > 0)
            {
                var _path = @$"{_hostEnvironment.ContentRootPath}{Resource.ContentImagePath}{_datetime}";
                Directory.CreateDirectory(_path);

                string filePathWithFileName = $@"{_path}\{model.FormFile.FileName}";

                using (Stream fileStream = new FileStream(filePathWithFileName, FileMode.Create))
                {
                    await model.FormFile.CopyToAsync(fileStream);
                }

                //spirtPDF to convert the PDF to images
                if (model.FormFile.ContentType.ToLower().Contains(Resource.PDFFileExtension))
                {
                    PdfDocument pdfDocument = new PdfDocument();
                    pdfDocument.LoadFromFile(filePathWithFileName);

                    for (int _counter = 0; _counter < pdfDocument.Pages.Count; ++_counter)
                    {
                        Image image = pdfDocument.SaveAsImage(_counter, PdfImageType.Bitmap, 500, 500);
                        image.Save(@$"{_path}\{Resource.ImageFilePrefix}{_counter + 1}.{Resource.DestinationImageFileExtension}", ImageFormat.Jpeg);
                        model.ImageFiles.Add(@$"{_path}\{Resource.ImageFilePrefix}{_counter + 1}.{Resource.DestinationImageFileExtension}");
                    }
                }
                else
                {
                    model.ImageFiles.Add(filePathWithFileName);
                }
                //spirtPDF to convert the PDF to images
            }
            model.FilePath = $"{Resource.ImageFileAbsolutePath}{_datetime}";
        }

        private async Task<string> ProcessMapping(List<GeminiResponse> jsonMapping, string imageFile)
        {
            Image image = Image.FromFile(imageFile);
            Pen blackPen = new Pen(Color.Black, 3);
            string _filePath = Path.GetDirectoryName(imageFile);
            string _filename = Path.GetFileNameWithoutExtension(imageFile);
            string _designOutPath = @$"{_filePath}\{_filename}{Resource.FinalImageMappedPrefix}.{Resource.DestinationImageFileExtension}";

            var x = 0;
            var y = 0;
            var height = 0;
            var width = 0;
 
            try
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    foreach (var _docs in jsonMapping)
                    {
                        foreach (var item in _docs.Elements)
                        {
                            x = (item.X * image.Width) / 1000;
                            y = (item.Y * image.Height) / 1000;
                            height = (item.Height * image.Height) / 1000;
                            width = (item.Width * image.Width) / 1000;

                            g.DrawRectangle(blackPen, x, y, width, height);
                        }
                    }
                }
                image.Save(_designOutPath);
            }
            catch(Exception ex)
            {
                throw;
            }
            return _designOutPath;
        }


        private async Task<string> ProcessFiles(GeminiRequest model)
        {
            List<GeminiResponse> response = new List<GeminiResponse>();
            int pageNumber = 1;
            foreach (var file in model.ImageFiles)
            {
                var _responseData = await GetGeminiResponse(Resource.DocDataPrompt, file);
                if(_responseData == null)
                {
                    continue;
                }
                string _imageFileName = await ProcessMapping(_responseData, file);
                foreach (var item in _responseData)
                {
                    item.PageNumber = Convert.ToString(pageNumber++);
                    item.ImageName = Path.GetFileName(_imageFileName);
                    item.ContentType = model.ContentType;
                    item.FileName = model.FileName;
                    item.FilePath = model.FilePath;
                    response.Add(item);
                }
            }
            return JsonConvert.SerializeObject(response);
        }

        private async Task<List<GeminiResponse>> GetGeminiResponse(string prompt, string fileName)
        {
            try
            {
                HttpResponseMessage _response = null;
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    var fileBytes = System.IO.File.ReadAllBytes(fileName);
                    var fileContent = Convert.ToBase64String(fileBytes);
                    var jsonFile = GetPrompt(prompt, fileContent);
                    HttpContent content = new StringContent(jsonFile, Encoding.UTF8, Resource.ResponseMimeType);
                    _response = _httpClient.PostAsync(@$"{_configuration[Resource.DocVerifierEndPoint]}{_configuration[Resource.APIKey]}", content).Result;
                }
                var _responseData = _response.Content.ReadAsStringAsync().Result;
                var _json = JsonConvert.DeserializeObject<dynamic>(_responseData);
                var _text = _json?.candidates[0]?.content?.parts[0].text;
                return JsonConvert.DeserializeObject<List<GeminiResponse>>(_text.ToString());
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        private string GetPrompt(string prompt, string fileContent)
        {
            return "{'generationConfig':{'response_mime_type':'application/json'},'contents':[{'parts':[{'text':'" + prompt + "'},{'inline_data':{'mime_type':'image/jpeg','data':'" + fileContent + "'}}]}]}";
        }
    }
}
