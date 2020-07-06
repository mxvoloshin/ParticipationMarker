using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class UnpinChatMessage
    {
        [JsonProperty("chat_id")]
        public string ChatId { get; set; }
    }
}