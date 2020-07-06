using System;
using System.Linq;
using System.Threading.Tasks;
using ParticipationMarker.App.TelegramBot;
using ParticipationMarker.Common.Services;
using ParticipationMarker.Infrastrucutre.Occasion;
using ParticipationMarker.Infrastrucutre.Poll;
using ParticipationMarker.Infrastrucutre.PollAnswer;
using Telegram.API;
using Telegram.API.Domain;

namespace ParticipationMarker.App.Services.Poll
{
    public class PollService : IPollService
    {
        private readonly IPollRepository _pollRepository;
        private readonly IPollAnswerRepository _pollAnswerRepository;
        private readonly IDateTimeService _dateTimeService;
        private readonly ITelegramClient _telegramClient;
        private readonly IOccasionRepository _occasionRepository;

        public PollService(IPollRepository pollRepository,
                           IPollAnswerRepository pollAnswerRepository,
                           IDateTimeService dateTimeService,
                           ITelegramClient telegramClient,
                           IOccasionRepository occasionRepository)
        {
            _pollRepository = pollRepository;
            _pollAnswerRepository = pollAnswerRepository;
            _dateTimeService = dateTimeService;
            _telegramClient = telegramClient;
            _occasionRepository = occasionRepository;
        }

        public async Task StartAsync(UpdateMessage message)
        {
            var callbackQueryData = message.CallbackQuery.Data.Split(';');
            var occasionId = callbackQueryData[1];

            var occasion = _occasionRepository.FindById(occasionId);
            if (occasion == null)
            {
                return;
            }

            var activePolls = _pollRepository.Find(x => x.ChatId == occasion.ChatId && !x.IsClosed).ToList();
            if (activePolls.Any())
            {
                await _telegramClient.SendMessageAsync(new SendMessage
                {
                    ChatId = occasion.ChatId,
                    Text = Messages.ActivePollRunning
                });

                return;
            }

            var sendPollMessage = TelegramMessageFactory.CreateSendPollMessage(occasion.ChatId, occasion.Name);
            var pollMessage = await _telegramClient.SendPollAsync(sendPollMessage);

            var entity = new PollEntity
            {
                PartitionKey = occasion.ChatId,
                RowKey = pollMessage.Poll.Id,
                Timestamp = _dateTimeService.TableEntityTimeStamp,
                ChatId = occasion.ChatId,
                OccasionId = occasionId,
                MessageId = pollMessage.MessageId.ToString(),
                IsClosed = false
            };

            await _pollRepository.CreateAsync(entity);

            await _telegramClient.AnswerCallbackQuery(new AnswerCallbackQuery
            {
                CallBackQueryId = message.CallbackQuery.Id,
                Text = Messages.PollStarted
            });
        }

        public async Task StopAllActivePollsAsync(UpdateMessage message)
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

            var activePolls = _pollRepository.Find(x => x.ChatId == chatId && !x.IsClosed).ToList();
            if (!activePolls.Any())
            {
                await _telegramClient.SendMessageAsync(new SendMessage
                {
                    ChatId = chatId,
                    Text = Messages.NoActivePollRunning
                });

                return;
            }

            var occasions = _occasionRepository.GetByChatId(chatId).ToList();
            foreach (var activePoll in activePolls)
            {
                var stopPollMessage =
                    TelegramMessageFactory.CreateStopPollMessage(activePoll.ChatId, activePoll.MessageId);
                await _telegramClient.StopPollAsync(stopPollMessage);

                activePoll.IsClosed = true;
                await _pollRepository.UpdateAsync(activePoll);

                var pollOccasion = occasions.FirstOrDefault(x => x.RowKey == activePoll.OccasionId);
                if (pollOccasion == null)
                {
                    continue;
                }

                await _telegramClient.SendMessageAsync(new SendMessage
                {
                    ChatId = chatId,
                    Text = string.Format(Messages.ActivePollStopped, pollOccasion.Name)
                });
            }
        }

        public async Task AddAnswerAsync(UpdateMessage message)
        {
            var pollId = message.PollAnswer.PollId;
            var userId = message.PollAnswer.User.Id.ToString();
            var isYesAnswer = message.PollAnswer.OptionIds.First() == 0;

            var pollEntity = _pollRepository.Find(x => x.RowKey == pollId).FirstOrDefault();
            if (pollEntity == null)
            {
                return;
            }

            var entity = new PollAnswerEntity
            {
                PartitionKey = pollEntity.ChatId,
                RowKey = Guid.NewGuid().ToString(),
                Timestamp = _dateTimeService.TableEntityTimeStamp,
                PollId = pollId,
                UserId = userId,
                IsYesAnswer = isYesAnswer
            };

            await _pollAnswerRepository.CreateAsync(entity);
        }
    }
}