namespace ParticipationMarker.TelegramBot
{
    public static class BotCommands
    {
        public static string CreateAction = @"/create_action@participation_marker_bot"; // Create new action to participate
        public static string DeleteAction = @"/delete_action@participation_marker_bot"; // Remove action from the list of actions
        public static string StartActionPoll = @"/start_action_poll@participation_marker_bot"; // Start poll to gather action participants
        public static string StopActionPoll = @"/stop_action_poll@participation_marker_bot"; // Stop active poll
    }
}