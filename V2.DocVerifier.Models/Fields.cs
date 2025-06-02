using Newtonsoft.Json;


namespace V2.DocVerifier.Models
{
    public class Elements
    {
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "confidence_score")]
        public float Confidence_Score { get; set; }

        [JsonProperty(PropertyName = "y")]
        public int Y { get; set; }

        [JsonProperty(PropertyName = "x")]
        public int X { get; set; }

        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }
    }
}
