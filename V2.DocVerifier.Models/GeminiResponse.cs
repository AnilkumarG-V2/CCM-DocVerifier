using Newtonsoft.Json;

namespace V2.DocVerifier.Models
{
    public class GeminiResponse : GeminiBaseModel
    {
        [JsonProperty(PropertyName = "elements")]
        public List<Elements> Elements { get; set; }
    }
}
