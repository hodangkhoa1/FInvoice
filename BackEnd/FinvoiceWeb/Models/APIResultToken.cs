using Newtonsoft.Json;

namespace FinvoiceWeb.Models
{
    public class APIResultToken
    {
        [JsonProperty("data")]
        public AccountToken Data { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
