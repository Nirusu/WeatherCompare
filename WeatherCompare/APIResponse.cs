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
        public Body[] Body { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("time_exec")]
        public float TimeToExec { get; set; }
        [JsonProperty("time_server")]
        public int TimeServer { get; set; }
    }
    public class Body
    {
        [JsonProperty("_id")]
        public string StationID { get; set; }
        [JsonProperty("place")]
        public Place Place { get; set; }
        [JsonProperty("mark")]
        public int Mark { get; set; }
        [JsonProperty("measures")]
        public Dictionary<string, SingleModule> Measures { get; set; }
        [JsonProperty("modules")]
        public string[] Modules { get; set; }
    }
    public class Place
    {
        [JsonProperty("location")]
        public float[] Coordinates { get; set; }
        [JsonProperty("altitude")]
        public float Altitude { get; set; }
        [JsonProperty("timezone")]
        public string Timezone { get; set; }
    }
    public class SingleModule
    {
        [JsonProperty("res")]
        public Dictionary<string, double[]> MeasuredData { get; set; }
        [JsonProperty("type")]
        public string[] TypeOfModule { get; set; }
    }
}
