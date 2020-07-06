using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class Poll
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}