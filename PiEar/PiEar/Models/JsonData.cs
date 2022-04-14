using Newtonsoft.Json;

namespace PiEar.Models
{
    public class JsonData
    {
        [JsonProperty("channel_name")]
        public string ChannelName { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("bpm")]
        public int Bpm { get; set; }
        [JsonProperty("channel_count")]
        public int ChannelCount { get; set; }
    }
}