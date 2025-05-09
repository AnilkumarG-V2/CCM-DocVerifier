using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V2.DocVerifier.Models
{
    public class GeminiResponse
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "fileName")]
        public string FileName { get; set; }

        [JsonProperty(PropertyName = "filePath")]
        public string FilePath { get; set; }

        [JsonProperty(PropertyName = "contentType")]
        public string ContentType { get; set; }

        [JsonProperty(PropertyName = "documentType")]
        public string DocumentType { get; set; }

        [JsonProperty(PropertyName = "pageNumber")]
        public string PageNumber { get; set; }

        [JsonProperty(PropertyName = "elements")]
        public List<Elements> Elements { get; set; }

        [JsonProperty(PropertyName = "imageName")]
        public string ImageName { get; set; }

        [JsonProperty(PropertyName = "confidenceScore")]
        public float ConfidenceScore { get; set; }
    }
}
