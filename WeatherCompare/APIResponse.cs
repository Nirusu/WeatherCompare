using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherCompare
{
    public class APIResponse
    {
        [JsonProperty("body")]
        public Body[] body { get; set; }
        [JsonProperty("status")]
        public string status { get; set; }
        [JsonProperty("time_exec")]
        public float timeToExec { get; set; }
        [JsonProperty("time_server")]
        public int timeServer { get; set; }
    }
    public class Body
    {
        [JsonProperty("_id")]
        public string stationID { get; set; }
        [JsonProperty("place")]
        public Place place { get; set; }
        [JsonProperty("mark")]
        public int mark { get; set; }
        [JsonProperty("measures")]
        public Dictionary<string, SingleModule> measures { get; set; }
        [JsonProperty("modules")]
        public string[] modules { get; set; }
    }
    public class Place
    {
        [JsonProperty("location")]
        public float[] coordinates { get; set; }
        [JsonProperty("altitude")]
        public float altitude { get; set; }
        [JsonProperty("timezone")]
        public string timezone { get; set; }
    }
    public class SingleModule
    {
        [JsonProperty("res")]
        public Dictionary<string, double[]> measuredData { get; set; }
        [JsonProperty("type")]
        public string[] typeOfModule { get; set; }
    }
}
