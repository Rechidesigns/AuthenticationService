using Newtonsoft.Json;


namespace AuthService.Data.UserDatas.DTOs
{
    public class MetaData
    {/// <summary>
     /// The current page number
     /// </summary>
     /// 
        [JsonProperty("page_index")]
        public int PageIndex { get; set; }

        /// <summary>
        /// The number of items on a page
        /// </summary>
        /// 
        [JsonProperty("page_size")]
        public int PageSize { get; set; }

        /// <summary>
        /// The total number of items
        /// </summary>
        /// 
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        /// <summary>
        /// The total number of pages
        /// </summary>
        /// 
        [JsonProperty("number_of_pages")]
        public int TotalPages { get; set; }
    }
}

