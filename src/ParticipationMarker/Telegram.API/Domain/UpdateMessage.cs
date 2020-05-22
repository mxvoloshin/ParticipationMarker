using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class UpdateMessage
    {
        [JsonProperty("update_id")]
        public long UpdateId { get; set; }

        [JsonProperty("message")]
        public Message Message { get; set; }
    }
}