﻿using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class InlineKeyboardButton
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("callback_data")]
        public string CallbackData { get; set; }
    }
}