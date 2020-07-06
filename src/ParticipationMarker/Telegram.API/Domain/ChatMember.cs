using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class ChatMember
    {
        [JsonProperty("user")]
        public User User { get; set; }
    }
}