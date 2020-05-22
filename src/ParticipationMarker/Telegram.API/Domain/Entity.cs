using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class Entity
    {
        [JsonProperty("offset")]
        public long Offset { get; set; }

        [JsonProperty("length")]
        public long Length { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}