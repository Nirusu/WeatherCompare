using Newtonsoft.Json;
namespace WeatherCompare
{
        public class AccessData
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; }
            [JsonProperty("scope")]
            public string[] Scope { get; set; }
            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }
        }
}
