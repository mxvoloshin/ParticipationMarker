using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Telegram.API.Domain
{
    public class SendMessage
    {
        [JsonProperty("chat_id")]
        public string ChatId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("parse_mode")]
        public string ParseMode { get; set; }

        [JsonProperty("reply_to_message_id")]
        public int? ReplyToMessageId { get; set; }

        [JsonProperty("reply_markup")]
        public JObject ReplyMarkup { get; set; }
    }
}