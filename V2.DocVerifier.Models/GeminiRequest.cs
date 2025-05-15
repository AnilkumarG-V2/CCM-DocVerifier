using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace V2.DocVerifier.Models
{
    public class GeminiViewModel
    {
        public GeminiViewModel(GeminiRequest request)
        {
            FormFile = request.FormFile;
        }

        [DisplayName("Select Form")]
        public IFormFile FormFile { get; set; }

        [JsonIgnore]
        public string FilePath { get; set; }

        [JsonIgnore]
        public string FileName { get; set; }

        [JsonIgnore]
        public List<string> ImageFiles { get; set; } = new List<string>();
    }

    public class GeminiRequest
    {
        [DisplayName("Select Form")]
        public IFormFile FormFile { get; set; }
    }
}
