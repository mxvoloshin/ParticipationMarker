using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class ResponseChat
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }

        [JsonProperty("result")]
        public Chat Result { get; set; }
    }
}