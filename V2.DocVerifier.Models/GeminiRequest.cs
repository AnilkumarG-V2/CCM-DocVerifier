using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace V2.DocVerifier.Models
{
    public class GeminiRequest
    {
        public string Name { get; set; }

        [DisplayName("Select Form")]
        public IFormFile FormFile { get; set; }

        public string FilePath { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }

        public GeminiResponse FormResponse { get; set; }

        public List<string> ImageFiles { get; set; } = new List<string>();

        public string Json { get; set; }
    }
}
