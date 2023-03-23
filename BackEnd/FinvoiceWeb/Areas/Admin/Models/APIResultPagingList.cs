using FinvoiceWeb.Models;
using Newtonsoft.Json;

namespace FinvoiceWeb.Areas.Admin.Models
{
    public class APIResultPagingList
    {
        [JsonProperty("data")]
        public List<ListUserAPI> Data { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("endPage")]
        public int EndPage { get; set; }
        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }
    }
}
