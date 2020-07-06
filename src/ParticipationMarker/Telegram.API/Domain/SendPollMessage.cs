using System.Collections.Generic;
using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class SendPollMessage
    {
        [JsonProperty("chat_id")]
        public string ChatId { get; set; }

        [JsonProperty("question")]
        public string Question { get; set; }

        [JsonProperty("options")]
        public List<string> Options { get; set; }

        [JsonProperty("is_anonymous")]
        public bool IsAnonymous { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("allows_multiple_answers")]
        public bool AllowMultipleAnswers { get; set; }

        [JsonProperty("open_period")]
        public int? OpenPeriod { get; set; }

        [JsonProperty("close_date")]
        public int? CloseDate { get; set; }

        [JsonProperty("is_closed")]
        public bool? IsClosed { get; set; }
    }
}