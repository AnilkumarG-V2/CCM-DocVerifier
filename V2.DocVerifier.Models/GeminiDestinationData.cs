using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V2.DocVerifier.Models
{
    public class GeminiDestinationData : GeminiBaseModel
    {
        [JsonProperty(PropertyName = "elements")]
        public DataElements Elements { get; set; }
    }

    public class DataElements
    {
        [JsonProperty(PropertyName = "employee_social_security_number")]
        public DataAttributes Employee_Social_Security_Number { get; set; }

        [JsonProperty(PropertyName = "employer_identification_number")]
        public DataAttributes Employer_Identification_Number { get; set; }

        [JsonProperty(PropertyName = "employer_name")]
        public DataAttributes Employer_Name { get; set; }

        [JsonProperty(PropertyName = "employer_city")]
        public DataAttributes Employer_City { get; set; }

        [JsonProperty(PropertyName = "employer_zip_code")]
        public DataAttributes Employer_Zip_Code { get; set; }
    }

    public class DataAttributes
    {
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "sourceValue")]
        public string SourceValue { get; set; }

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
