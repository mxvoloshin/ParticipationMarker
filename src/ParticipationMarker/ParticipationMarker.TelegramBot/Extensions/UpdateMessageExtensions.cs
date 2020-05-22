using System;
using ParticipationMarker.Domain.Events;
using Telegram.API.Domain;

namespace ParticipationMarker.TelegramBot.Extensions
{
    public static class UpdateMessageExtensions
    {
        public static DeleteActionEvent IsDeleteActionCommand(this UpdateMessage updateMessage)
        {
            if (updateMessage.Message == null || string.IsNullOrEmpty(updateMessage.Message.Text))
            {
                return null;
            }

            if (!string.Equals(updateMessage.Message.Text, BotCommands.DeleteAction,
                StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return new DeleteActionEvent
            {
                ChatId = updateMessage.Message.Chat.Id
            };
        }
    }
}