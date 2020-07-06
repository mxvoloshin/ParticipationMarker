using System.Collections.Generic;
using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class PollAnswer
    {
        [JsonProperty("poll_id")]
        public string PollId { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("option_ids")]
        public List<int> OptionIds { get; set; }
    }
}