using Newtonsoft.Json;
using ParticipationMarker.TelegramBot.Extensions;
using Telegram.API.Domain;

namespace ParticipationMarker.TelegramBot
{
    public class InputParser
    {
        public static void ParseRequest(string requestBody)
        {
            var updateMessage = JsonConvert.DeserializeObject<UpdateMessage>(requestBody);
            
            var deleteActionEvent = updateMessage.IsDeleteActionCommand();
            if (deleteActionEvent != null)
            {
                // send event to eventgrid
            }
        }
    }
}