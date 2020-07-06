using System.Threading.Tasks;
using Telegram.API.Domain;

namespace ParticipationMarker.App.Services.Poll
{
    public interface IPollService
    {
        Task StartAsync(UpdateMessage message);
        Task StopAllActivePollsAsync(UpdateMessage message);
        Task AddAnswerAsync(UpdateMessage message);
    }
}