﻿using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class Message
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

        [JsonProperty("entities")]
        public Entity[] Entities { get; set; }

        [JsonProperty("reply_to_message")]
        public ReplyToMessage ReplyToMessage { get; set; }

        [JsonProperty("poll")]
        public Poll Poll { get; set; }
    }
}