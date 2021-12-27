using Newtonsoft.Json;

namespace Scoreboard.Client
{
    public class SignalRConnectionInfo
    {
        [JsonProperty(PropertyName = "url")]
        public string? Url { get; set; }
        [JsonProperty(PropertyName = "accessToken")]
        public string? AccessToken { get; set; }
    }
}
