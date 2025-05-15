using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace V2.DocVerifier.Models
{
    public class GeminiPrompt
    {
        [JsonProperty("generationConfig")]
        public GeneratorConfig GeneratorConfig { get; set; } = new GeneratorConfig();

        [JsonProperty("contents")]
        public List<Content> Contents { get; set; } = new List<Content>();
    }

    public class GeneratorConfig
    {
        [JsonProperty("response_mime_type")]
        public string ResponseMIMIType { get; set; } = "application/json";
    }

    public class Content
    {
        [JsonProperty("parts")]
        public List<object> Parts { get; set; } = new List<object>();
    }

    public class Part
    {
        public List<object> Data { get; set; } = new List<object>();
    }

    public class InlineData
    {
        [JsonProperty("mime_type")]
        public string MimeType { get; set; } = "image/jpeg";

        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
