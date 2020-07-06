namespace ParticipationMarker.App.Enums
{
    public enum UpdateMessageType
    {
        Unknown = -1,
        GetNewOccasionName = 0,
        CreateOccasion = 1,
        GetOccasionsForDelete = 2,
        DeleteOccasion = 3,
        GetOccasionsToStart = 4,
        StartPoll = 5,
        StopActivePoll = 6,
        PollAnswered = 7,
        Stats = 8,
    }
}