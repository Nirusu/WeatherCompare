using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using WeatherCompare.Properties;
using System.Globalization;

namespace WeatherCompare
{
    class NetatmoSession
    {
        HttpClient httpClient = new HttpClient();
        HttpResponseMessage Response;
        HttpContent Content;
        //TODO: Create dialog for entering Netatmo Account
        private const String authUsername = ""; // HARDCODED - REPLACE ME
        private const String authPassword = ""; // HARDCODED - REPLACE ME
        private String clientID = Properties.Settings.Default.ClientId;
        private String clientSecret = Properties.Settings.Default.ClientSecret;
        public NetatmoSession()
        {
        }
        public async Task<AccessData> GetAccessTokenFromPassword()
        {
            StringContent authPostContent = new StringContent("grant_type=password&client_id=" + clientID + "&client_secret=" + clientSecret + "&username=" + authUsername + "&password=" + authPassword + "&scope=read_station", UnicodeEncoding.UTF8, "application/x-www-form-urlencoded");
            Response = await httpClient.PostAsync("https://api.netatmo.com/oauth2/token", authPostContent);
            Content = Response.Content;
            AccessData accessData = JsonConvert.DeserializeObject<AccessData>(await Content.ReadAsStringAsync());
            return accessData;
        }
        public async Task<AccessData> RefreshToken()
        {
            StringContent refreshAuthPostContent = new StringContent("grant_type=refresh_token&refresh_token=" + Settings.Default.RefreshToken + "&client_id=" + Settings.Default.ClientId + "&client_secret=" + Settings.Default.ClientSecret + "&scope=read_station", UnicodeEncoding.UTF8, "application/x-www-form-urlencoded");
            Response = await httpClient.PostAsync("https://api.netatmo.com/oauth2/token", refreshAuthPostContent);
            Content = Response.Content;
            AccessData accessData = JsonConvert.DeserializeObject<AccessData>(await Content.ReadAsStringAsync());
            return accessData;
        }
        public async Task<APIResponse> RequestPublicWeatherStationData(String accesstoken, double lat_ne, double lon_ne, double lat_sw, double lon_sw)
        {
            StringContent publicDataPostContent = new StringContent("access_token=" + accesstoken + "&lat_ne=" + lat_ne.ToString(CultureInfo.InvariantCulture) + "&lon_ne=" + lon_ne.ToString(CultureInfo.InvariantCulture) + "&lat_sw=" + lat_sw.ToString(CultureInfo.InvariantCulture) + "&lon_sw=" + lon_sw.ToString(CultureInfo.InvariantCulture) + "&required_data=temperature&filter=false", UnicodeEncoding.UTF8, "application/x-www-form-urlencoded");
            Response = await httpClient.PostAsync("https://api.netatmo.com/api/getpublicdata", publicDataPostContent);
            Content = Response.Content;
            APIResponse apiResponse = JsonConvert.DeserializeObject<APIResponse>(await Content.ReadAsStringAsync());
            return apiResponse;
        }
    }
}
