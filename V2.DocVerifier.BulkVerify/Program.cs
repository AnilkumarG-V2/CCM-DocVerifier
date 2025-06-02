using Newtonsoft.Json;
using System.Net.Http.Headers;
using V2.DocVerifier.Models;

string[] files = Directory.GetFiles(@"D:\Products\V2Competency\textract_lending_analysis_poc-develop\textract_lending_analysis_poc-develop\sample_data\Kanhai");
int counter = 1;


foreach (string file in files)
{
    Console.WriteLine($"Processing file # {1} with file name {Path.GetFileName(file)}");
    try
    {
        HttpResponseMessage _response = null;
        HttpClient _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(@"http://localhost:5079/");

        using (var filestream = File.OpenRead(file))
        {
            var length = filestream.Length.ToString();
            var streamContent = new StreamContent(filestream);
            streamContent.Headers.Add("Content-Type", "application/pdf");
            streamContent.Headers.Add("Content-Length", length);
            var content = new MultipartFormDataContent();
            content.Add(streamContent, "FormFile", Path.GetFileName(file));
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "FormFile", FileName = Path.GetFileName(file) };
            _response = await _httpClient.PostAsync("DocVerifier", content);
            var _responseData = await _response.Content.ReadAsStringAsync();
            List<GeminiResponse> _responseObject = JsonConvert.DeserializeObject<List<GeminiResponse>>(_responseData.ToString());

            string _resultFolderPath = @"D:\PaystubResults\";
            File.WriteAllText($"{_resultFolderPath}{Path.GetFileNameWithoutExtension(file)}.json", JsonConvert.SerializeObject(_responseObject));    
        }
    }
    catch(Exception ex)
    {
        Console.WriteLine($"Failed : Processing file # {1} with file name {Path.GetFileName(file)}");
    }
}
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
