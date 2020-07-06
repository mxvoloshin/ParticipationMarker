using System.Collections.Generic;
using System.Linq;

namespace ParticipationMarker.Infrastrucutre.Occasion
{
    public class OccasionRepository : BaseRepository<OccasionEntity>, IOccasionRepository
    {
        public OccasionRepository(ICloudStorageSettings cloudStorageSettings) : base(cloudStorageSettings, "Occasions")
        {

        }

        public IEnumerable<OccasionEntity> GetByChatId(string chatId)
        {
            return Find(x => x.ChatId == chatId).ToList();
        }
    }
}