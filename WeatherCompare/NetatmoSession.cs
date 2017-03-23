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
        HttpResponseMessage response;
        HttpContent content;
        //TODO: Create dialog for entering Netatmo Account
        const String authUsername = ""; // REPLACE ME
        const String authPassword = ""; // REPLACE ME
        String clientID = Properties.Settings.Default.clientID;
        String clientSecret = Properties.Settings.Default.clientSecret;
        public NetatmoSession()
        {
        }
        public async Task<AccessData> getAccessTokenFromPassword()
        {
            StringContent authPostContent = new StringContent("grant_type=password&client_id=" + clientID + "&client_secret=" + clientSecret + "&username=" + authUsername + "&password=" + authPassword + "&scope=read_station", UnicodeEncoding.UTF8, "application/x-www-form-urlencoded");
            response = await httpClient.PostAsync("https://api.netatmo.com/oauth2/token", authPostContent);
            content = response.Content;
            AccessData accessdata = JsonConvert.DeserializeObject<AccessData>(await content.ReadAsStringAsync());
            return accessdata;
        }
        public async Task<AccessData> refreshToken()
        {
            StringContent refreshAuthPostContent = new StringContent("grant_type=refresh_token&refresh_token=" + Settings.Default.refresh_token + "&client_id=" + Settings.Default.clientID + "&client_secret=" + Settings.Default.clientSecret + "&scope=read_station", UnicodeEncoding.UTF8, "application/x-www-form-urlencoded");
            response = await httpClient.PostAsync("https://api.netatmo.com/oauth2/token", refreshAuthPostContent);
            content = response.Content;
            AccessData accessdata = JsonConvert.DeserializeObject<AccessData>(await content.ReadAsStringAsync());
            return accessdata;
        }
        public async Task<APIResponse> requestPublicWeatherStationData(String accesstoken, double lat_ne, double lon_ne, double lat_sw, double lon_sw)
        {
            StringContent publicDataPostContent = new StringContent("access_token=" + accesstoken + "&lat_ne=" + lat_ne.ToString(CultureInfo.InvariantCulture) + "&lon_ne=" + lon_ne.ToString(CultureInfo.InvariantCulture) + "&lat_sw=" + lat_sw.ToString(CultureInfo.InvariantCulture) + "&lon_sw=" + lon_sw.ToString(CultureInfo.InvariantCulture) + "&required_data=temperature&filter=false", UnicodeEncoding.UTF8, "application/x-www-form-urlencoded");
            response = await httpClient.PostAsync("https://api.netatmo.com/api/getpublicdata", publicDataPostContent);
            content = response.Content;
            APIResponse apiResponse = JsonConvert.DeserializeObject<APIResponse>(await content.ReadAsStringAsync());
            return apiResponse;
        }
    }
}
