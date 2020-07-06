using System.Threading.Tasks;
using Telegram.API.Domain;

namespace ParticipationMarker.App.Services.Occasion
{
    public interface IOccasionService
    {
        Task RequestNewOccasionNameAsync(UpdateMessage message);
        Task CreateAsync(UpdateMessage message);
        Task DeleteAsync(UpdateMessage message);
        Task GetOccasionsForDeleteAsync(UpdateMessage message);
        Task GetOccasionsToStartAsync(UpdateMessage message);
    }
}