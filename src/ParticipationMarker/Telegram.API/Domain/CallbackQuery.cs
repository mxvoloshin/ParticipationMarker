using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class CallbackQuery
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("from")]
        public CallbackQueryFrom From { get; set; }

        [JsonProperty("message")]
        public Message Message { get; set; }

        [JsonProperty("chat_instance")]
        public string ChatInstance { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
    }
}