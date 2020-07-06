using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class ReplyToMessage
    {
        [JsonProperty("message_id")]
        public long MessageId { get; set; }

        [JsonProperty("from")]
        public User From { get; set; }

        [JsonProperty("chat")]
        public Chat Chat { get; set; }

        [JsonProperty("date")]
        public long Date { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}