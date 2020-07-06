using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class ForceReply
    {
        [JsonProperty("force_reply")]
        public bool Force { get; set; }
        [JsonProperty("selective")]
        public bool Selective { get; set; }
    }
}