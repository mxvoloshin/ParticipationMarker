using System;
using System.Linq;
using ParticipationMarker.App.Enums;
using ParticipationMarker.App.TelegramBot;
using Telegram.API.Domain;

namespace ParticipationMarker.App.Extensions
{
    public static class UpdateMessageExtensions
    {
        public static UpdateMessageType ParseMessageType(this UpdateMessage message)
        {
            if (message.Message != null)
            {
                var messageText = message.Message.Text;
                if (message.IsBotCommand())
                {
                    if (messageText.StartsWith(BotCommands.CreateOccasion, StringComparison.OrdinalIgnoreCase))
                    {
                        return UpdateMessageType.GetNewOccasionName;
                    }

                    if (messageText.StartsWith(BotCommands.DeleteOccasion, StringComparison.OrdinalIgnoreCase))
                    {
                        return UpdateMessageType.GetOccasionsForDelete;
                    }

                    if (messageText.StartsWith(BotCommands.StartPoll, StringComparison.OrdinalIgnoreCase))
                    {
                        return UpdateMessageType.GetOccasionsToStart;
                    }

                    if (messageText.StartsWith(BotCommands.StopPoll, StringComparison.OrdinalIgnoreCase))
                    {
                        return UpdateMessageType.StopActivePoll;
                    }

                    if (messageText.StartsWith(BotCommands.Stats, StringComparison.OrdinalIgnoreCase))
                    {
                        return UpdateMessageType.Stats;
                    }
                }
            }

            var replyToMessage = message.Message?.ReplyToMessage;
            if (replyToMessage != null)
            {
                if (replyToMessage.From.IsBot)
                {
                    if (replyToMessage.Text.Contains(Messages.NewOccasionName, StringComparison.OrdinalIgnoreCase))
                    {
                        return UpdateMessageType.CreateOccasion;
                    }
                }
            }

            var callbackQuery = message.CallbackQuery;
            if (callbackQuery != null)
            {
                var callbackQueryData = callbackQuery.Data.Split(';');

                if (callbackQueryData.Length == 2 && callbackQueryData[0] == BotCommands.DeleteOccasion)
                {
                    return UpdateMessageType.DeleteOccasion;
                }

                if (callbackQueryData.Length == 2 && callbackQueryData[0] == BotCommands.StartPoll)
                {
                    return UpdateMessageType.StartPoll;
                }
            }

            var pollAnswer = message.PollAnswer;
            if (pollAnswer != null)
            {
                return UpdateMessageType.PollAnswered;
            }

            return UpdateMessageType.Unknown;
        }

        private static bool IsBotCommand(this UpdateMessage updateMessage)
        {
            return updateMessage.Message?.Entities != null && updateMessage.Message.Entities.Any(x => x.Type == "bot_command");
        }
    }
}