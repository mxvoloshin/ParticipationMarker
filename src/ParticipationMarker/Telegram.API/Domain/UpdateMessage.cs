using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class UpdateMessage
    {
        [JsonProperty("update_id")]
        public long UpdateId { get; set; }

        [JsonProperty("message")]
        public Message Message { get; set; }

        [JsonProperty("callback_query")]
        public CallbackQuery CallbackQuery { get; set; }

        [JsonProperty("poll_answer")]
        public PollAnswer PollAnswer { get; set; }
    }
}