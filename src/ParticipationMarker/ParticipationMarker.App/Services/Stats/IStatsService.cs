using System.Threading.Tasks;
using Telegram.API.Domain;

namespace ParticipationMarker.App.Services.Stats
{
    public interface IStatsService
    {
        Task ShowAsync(UpdateMessage message);
    }
}