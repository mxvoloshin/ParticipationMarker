using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class PinChatMessage
    {
        [JsonProperty("chat_id")]
        public string ChatId { get; set; }

        [JsonProperty("message_id")]
        public string MessageId { get; set; }

        [JsonProperty("unpinChatMessage")]
        public bool DisableNotifications { get; set; }
    }
}