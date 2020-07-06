using System.Threading.Tasks;

namespace ParticipationMarker.App.TelegramBot
{
    public interface ITelegramInputParser
    {
        Task ParseRequestAsync(string requestBody);
    }
}