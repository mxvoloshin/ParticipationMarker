using System.Collections.Generic;
using Newtonsoft.Json;

namespace Telegram.API.Domain
{
    public class ResponseChatMembers
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }

        [JsonProperty("result")]
        public List<ChatMember> Result { get; set; }
    }
}