using System.Collections.Generic;
using System.Threading.Tasks;
using ParticipationMarker.App.Domain;
using Telegram.API.Domain;

namespace ParticipationMarker.App.Services.Stats
{
    public interface IStatsService
    {
        Task ShowAsync(UpdateMessage message);
        Task<IEnumerable<ChatMemberStat>> GetChatStatAsync(string chatName);
    }
}