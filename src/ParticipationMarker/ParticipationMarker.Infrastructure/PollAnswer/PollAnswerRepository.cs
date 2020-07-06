namespace ParticipationMarker.Infrastrucutre.PollAnswer
{
    public class PollAnswerRepository : BaseRepository<PollAnswerEntity>, IPollAnswerRepository
    {
        public PollAnswerRepository(ICloudStorageSettings cloudStorageSettings) : base(cloudStorageSettings, "PollAnswer")
        {
        }
    }
}