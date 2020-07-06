using System.Collections.Generic;
using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class InlineKeyboardMarkup
    {
        [JsonProperty("inline_keyboard")]
        public List<List<InlineKeyboardButton>> Keyboard { get; set; }
    }
}