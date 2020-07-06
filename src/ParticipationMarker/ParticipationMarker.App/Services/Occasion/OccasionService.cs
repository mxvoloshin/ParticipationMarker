using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ParticipationMarker.App.TelegramBot;
using ParticipationMarker.Common.Services;
using ParticipationMarker.Infrastrucutre.Occasion;
using Telegram.API;
using Telegram.API.Domain;

namespace ParticipationMarker.App.Services.Occasion
{
    public class OccasionService : IOccasionService
    {
        private readonly IOccasionRepository _occasionRepository;
        private readonly IDateTimeService _dateTimeService;
        private readonly ITelegramClient _telegramClient;

        public OccasionService(IOccasionRepository occasionRepository, IDateTimeService dateTimeService, ITelegramClient telegramClient)
        {
            _occasionRepository = occasionRepository;
            _dateTimeService = dateTimeService;
            _telegramClient = telegramClient;
        }

        public async Task CreateAsync(UpdateMessage message)
        {
            var chatId = message.Message.Chat.Id;
            var name = message.Message.Text.Trim();

            var occasions = _occasionRepository.GetByChatId(chatId);
            if (occasions.Any(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase)))
            {
                await _telegramClient.SendMessageAsync(new SendMessage
                {
                    ChatId = chatId,
                    Text = Messages.OccasionAllreadyExists
                });

                return;
            }

            await _occasionRepository.CreateAsync(new OccasionEntity
            {
                PartitionKey = chatId,
                RowKey = Guid.NewGuid().ToString(),
                Timestamp = _dateTimeService.TableEntityTimeStamp,
                ChatId = chatId,
                Name = name
            });
        }

        public async Task DeleteAsync(UpdateMessage message)
        {
            var callbackQueryData = message.CallbackQuery.Data.Split(';');
            var occasionId = callbackQueryData[1];
            var messageId = message.CallbackQuery.Message.MessageId.ToString();

            var occasion = _occasionRepository.Find(x => x.RowKey == occasionId).FirstOrDefault();
            if (occasion == null)
            {
                return;
            }

            await _occasionRepository.DeleteAsync(occasion);

            var occasionsToDelete = _occasionRepository.GetByChatId(occasion.ChatId).ToList();
            var updateDeleteOccasionMessage = TelegramMessageFactory.UpdateDeleteOccasionMessage(occasion.ChatId,
                messageId,
                occasionsToDelete);
            await _telegramClient.EditMessageText(updateDeleteOccasionMessage);

            await _telegramClient.AnswerCallbackQuery(new AnswerCallbackQuery
            {
                CallBackQueryId = message.CallbackQuery.Id,
                Text = Messages.OccasionDeleted
            });
        }

        public async Task RequestNewOccasionNameAsync(UpdateMessage message)
        {
            var chatId = message.Message.Chat.Id;
            var fromId = message.Message.From.Id.ToString();

            var isAdmin = await _telegramClient.IsAdminAsync(chatId, fromId);
            if (!isAdmin)
            {
                await _telegramClient.SendMessageAsync(new SendMessage
                {
                    ChatId = chatId,
                    Text = Messages.OnlyAdminCanDoThis,
                });
                return;
            }

            await _telegramClient.SendMessageAsync(new SendMessage
            {
                ChatId = chatId,
                Text = $"[{Messages.NewOccasionName}](tg://user?id={message.Message.From.Id})",
                ReplyMarkup = JObject.FromObject(new ForceReply
                {
                    Force = true,
                    Selective = true
                }),
                ParseMode = $"MarkdownV2"
            });
        }

        public async Task GetOccasionsForDeleteAsync(UpdateMessage message)
        {
            var chatId = message.Message.Chat.Id;
            var fromId = message.Message.From.Id.ToString();

            var isAdmin = await _telegramClient.IsAdminAsync(chatId, fromId);
            if (!isAdmin)
            {
                await _telegramClient.SendMessageAsync(new SendMessage
                {
                    ChatId = chatId,
                    Text = Messages.OnlyAdminCanDoThis,
                });

                return;
            }

            var occasionsToDelete = _occasionRepository.GetByChatId(chatId).ToList();
            if (!occasionsToDelete.Any())
            {
                await _telegramClient.SendMessageAsync(new SendMessage
                {
                    ChatId = chatId,
                    Text = Messages.NoOccasionsToDelete
                });

                return;
            }

            var sendMessage = TelegramMessageFactory.CreateDeleteOccasionMessage(chatId, occasionsToDelete);
            await _telegramClient.SendMessageAsync(sendMessage);
        }

        public async Task GetOccasionsToStartAsync(UpdateMessage message)
        {
            var chatId = message.Message.Chat.Id;
            var fromId = message.Message.From.Id.ToString();

            var isAdmin = await _telegramClient.IsAdminAsync(chatId, fromId);
            if (!isAdmin)
            {
                await _telegramClient.SendMessageAsync(new SendMessage
                {
                    ChatId = chatId,
                    Text = Messages.OnlyAdminCanDoThis,
                });

                return;
            }

            var occasions = _occasionRepository.GetByChatId(chatId).ToList();
            if (!occasions.Any())
            {
                await _telegramClient.SendMessageAsync(new SendMessage
                {
                    ChatId = chatId,
                    Text = Messages.NoOccasionsToStart
                });

                return;
            }

            var sendMessage = TelegramMessageFactory.CreateStartOccasionMessage(chatId, occasions);
            await _telegramClient.SendMessageAsync(sendMessage);
        }
    }
}