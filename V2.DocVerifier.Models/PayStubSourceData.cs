using Newtonsoft.Json;

namespace V2.DocVerifier.Models
{
    public class PayStubSourceData
    {
        [JsonProperty(PropertyName = "employee_social_security_number")]
        public string Employee_Social_Security_Number { get; set; }

        [JsonProperty(PropertyName = "employer_identification_number")]
        public string Employer_Identification_Number { get; set; }

        [JsonProperty(PropertyName = "employer_name")]
        public string Employer_Name { get; set; }

        [JsonProperty(PropertyName = "employer_city")]
        public string Employer_City { get; set; }

        [JsonProperty(PropertyName = "employer_zip_code")]
        public string Employer_Zip_Code { get; set; }
    }
}
