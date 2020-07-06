using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ParticipationMarker.Infrastrucutre.Occasion;
using Telegram.API.Domain;

namespace ParticipationMarker.App.TelegramBot
{
    public static class TelegramMessageFactory
    {
        public static SendMessage CreateStartOccasionMessage(string chatId, IEnumerable<OccasionEntity> occasions)
        {
            var inlineKeyboardMarkup = new InlineKeyboardMarkup { Keyboard = new List<List<InlineKeyboardButton>>() };
            foreach (var occasionEntity in occasions)
            {
                var list = new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton
                    {
                        Text = occasionEntity.Name,
                        CallbackData = $"{BotCommands.StartPoll};{occasionEntity.RowKey}"
                    }
                };
                inlineKeyboardMarkup.Keyboard.Add(list);
            }

            return new SendMessage
            {
                ChatId = chatId,
                Text = Messages.StartOccasion,
                ReplyMarkup = JObject.FromObject(inlineKeyboardMarkup)
            };
        }

        public static SendMessage CreateDeleteOccasionMessage(string chatId, IEnumerable<OccasionEntity> occasions)
        {
            var inlineKeyboardMarkup = new InlineKeyboardMarkup { Keyboard = new List<List<InlineKeyboardButton>>() };
            foreach (var occasionEntity in occasions)
            {
                var list = new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton
                    {
                        Text = occasionEntity.Name,
                        CallbackData = $"{BotCommands.DeleteOccasion};{occasionEntity.RowKey}"
                    }
                };
                inlineKeyboardMarkup.Keyboard.Add(list);
            }

            return new SendMessage
            {
                ChatId = chatId,
                Text = Messages.DeleteOccasion,
                ReplyMarkup = JObject.FromObject(inlineKeyboardMarkup)
            };
        }

        public static EditMessageText UpdateDeleteOccasionMessage(string chatId, string messageId, IEnumerable<OccasionEntity> occasions)
        {
            var inlineKeyboardMarkup = new InlineKeyboardMarkup { Keyboard = new List<List<InlineKeyboardButton>>() };
            foreach (var occasionEntity in occasions)
            {
                var list = new List<InlineKeyboardButton>
                {
                    new InlineKeyboardButton
                    {
                        Text = occasionEntity.Name,
                        CallbackData = $"{BotCommands.DeleteOccasion};{occasionEntity.RowKey}"
                    }
                };
                inlineKeyboardMarkup.Keyboard.Add(list);
            }

            return new EditMessageText
            {
                ChatId = chatId,
                MessageId = messageId,
                Text = occasions.Any() ? Messages.DeleteOccasion : Messages.NoOccasionsToDelete,
                ReplyMarkup = JObject.FromObject(inlineKeyboardMarkup)
            };
        }

        public static SendPollMessage CreateSendPollMessage(string chatId, string occasionName)
        {
            return new SendPollMessage
            {
                ChatId = chatId,
                Question = string.Format(Messages.StartPollQuestion, occasionName),
                IsAnonymous = false,
                Options = new List<string>{"✅", "🚫"},
                Type = "regular",
                AllowMultipleAnswers = false
            };
        }

        public static StopPollMessage CreateStopPollMessage(string chatId, string messageId)
        {
            return new StopPollMessage
            {
                ChatId = chatId,
                MessageId = messageId
            };
        }
    }
}