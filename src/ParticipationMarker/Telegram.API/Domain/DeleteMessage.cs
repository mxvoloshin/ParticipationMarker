using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class DeleteMessage
    {
        [JsonProperty("chat_id")]
        public string ChatId { get; set; }
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }
}