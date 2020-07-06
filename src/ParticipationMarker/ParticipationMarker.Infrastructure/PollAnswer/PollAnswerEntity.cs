using Microsoft.Azure.Cosmos.Table;

namespace ParticipationMarker.Infrastrucutre.PollAnswer
{
    public class PollAnswerEntity : TableEntity
    {
        public string PollId { get; set; }
        public string UserId { get; set; }
        public bool IsYesAnswer { get; set; }
    }
}