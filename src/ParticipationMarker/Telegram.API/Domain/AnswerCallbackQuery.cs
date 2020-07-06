using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class AnswerCallbackQuery
    {
        [JsonProperty("callback_query_id")]
        public string CallBackQueryId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}