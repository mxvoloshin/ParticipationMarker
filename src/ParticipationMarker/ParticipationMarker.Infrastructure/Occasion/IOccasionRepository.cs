using System.Collections.Generic;

namespace ParticipationMarker.Infrastrucutre.Occasion
{
    public interface IOccasionRepository : IBaseRepository<OccasionEntity>
    {
        IEnumerable<OccasionEntity> GetByChatId(string chatId);
    }
}