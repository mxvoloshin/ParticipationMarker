namespace ParticipationMarker.Infrastrucutre.Poll
{
    public class PollRepository : BaseRepository<PollEntity>, IPollRepository
    {
        public PollRepository(ICloudStorageSettings cloudStorageSettings) : base(cloudStorageSettings, "Poll")
        {
        }
    }
}