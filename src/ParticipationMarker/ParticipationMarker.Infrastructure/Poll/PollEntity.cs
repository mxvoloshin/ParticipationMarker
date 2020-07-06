using Microsoft.Azure.Cosmos.Table;

namespace ParticipationMarker.Infrastrucutre.Poll
{
    public class PollEntity : TableEntity
    {
        public string ChatId { get; set; }
        public string OccasionId { get; set; }
        public bool IsClosed { get; set; }
        public string MessageId { get; set; }
    }
}