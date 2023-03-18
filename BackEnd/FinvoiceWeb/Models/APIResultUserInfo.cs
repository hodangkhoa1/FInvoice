using Newtonsoft.Json;

namespace FinvoiceWeb.Models
{
    public class APIResultUserInfo
    {
        [JsonProperty("data")]
        public UserInfo Data { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
