using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class ResponseChatMember
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }

        [JsonProperty("result")]
        public ChatMember Result { get; set; }
    }
}