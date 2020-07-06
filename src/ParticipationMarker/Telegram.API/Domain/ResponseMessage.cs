using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class ResponseMessage
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }

        [JsonProperty("result")]
        public Message Result { get; set; }
    }
}