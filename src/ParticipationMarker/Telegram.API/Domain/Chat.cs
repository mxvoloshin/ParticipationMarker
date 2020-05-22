using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class Chat
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("all_members_are_administrators")]
        public bool AllMembersAreAdministrators { get; set; }
    }
}