using System;
using Newtonsoft.Json;

namespace Bird.Client.Mtchmkr.Business.ServiceCenter.Response
{
	public class GetMatchDetailResponse
	{
        [JsonProperty("userId")]
        public string userId { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("userName")]
        public string userName { get; set; }

        [JsonProperty("rating")]
        public int rating { get; set; }

        [JsonProperty("imageTitle")]
        public string imageTitle { get; set; }

        [JsonProperty("imageData")]
        public string imageData { get; set; }

        [JsonProperty("frameWinCount")]
        public int frameWinCount { get; set; }

        [JsonProperty("location")]
        public string location { get; set; }

        [JsonProperty("imageExtension")]
        public object imageExtension { get; set; }

        public int MaxFrame { get; set; }

    }
}

