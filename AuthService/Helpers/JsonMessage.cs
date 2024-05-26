
using Newtonsoft.Json;
using AuthService.Data.UserDatas.DTOs;
namespace AuthService.Helpers
{
    public class JsonMessage<T>
    {
        [JsonProperty("result")]
        public List<T> result { get; set; }

        [JsonProperty("status")]
        public bool status { get; set; }

        [JsonProperty("is_verified")]
        public bool is_verified { get; set; }

        [JsonProperty(PropertyName = "error_message")]
        public string error_message { get; set; }

        [JsonProperty("success_message")]
        public string success_message { get; set; }

        [JsonProperty("meta_data")]
        public MetaData meta_data { get; set; }
        [JsonProperty("status_code")]
        public int status_code { get; set; }
    }
}
