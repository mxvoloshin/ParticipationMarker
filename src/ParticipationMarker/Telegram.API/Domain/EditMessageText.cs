using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Telegram.API.Domain
{
    public class EditMessageText
    {
        [JsonProperty("chat_id")]
        public string ChatId { get; set; }

        [JsonProperty("message_id")]
        public string MessageId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("parse_mode")]
        public string ParseMode { get; set; }

        [JsonProperty("reply_markup")]
        public JObject ReplyMarkup { get; set; }
    }
}