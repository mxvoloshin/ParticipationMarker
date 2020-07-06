using Microsoft.Azure.Cosmos.Table;

namespace ParticipationMarker.Infrastrucutre.Occasion
{
    public class OccasionEntity : TableEntity
    {
        public string ChatId { get; set; }
        public string Name { get; set; }
    }
}