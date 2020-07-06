using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParticipationMarker.Infrastrucutre.Occasion;
using ParticipationMarker.Infrastrucutre.Poll;
using ParticipationMarker.Infrastrucutre.PollAnswer;
using Telegram.API;
using Telegram.API.Domain;

namespace ParticipationMarker.App.Services.Stats
{
    public class StatsService : IStatsService
    {
        private readonly IOccasionRepository _occasionRepository;
        private readonly IPollRepository _pollRepository;
        private readonly ITelegramClient _telegramClient;
        private readonly IPollAnswerRepository _pollAnswerRepository;

        public StatsService(IOccasionRepository occasionRepository, IPollRepository pollRepository, ITelegramClient telegramClient, IPollAnswerRepository pollAnswerRepository)
        {
            _occasionRepository = occasionRepository;
            _pollRepository = pollRepository;
            _telegramClient = telegramClient;
            _pollAnswerRepository = pollAnswerRepository;
        }
        public async Task ShowAsync(UpdateMessage message)
        {
            var chatId = message.Message.Chat.Id;
            var occasions = _occasionRepository.GetByChatId(chatId);
            var polls = new List<PollEntity>();
            foreach (var occasionEntity in occasions)
            {
                var occasionPolls = _pollRepository.Find(x=>x.OccasionId == occasionEntity.RowKey && x.IsClosed);
                polls.AddRange(occasionPolls);
            }

            if (!polls.Any())
            {
                await _telegramClient.SendMessageAsync(new SendMessage
                {
                    ChatId = chatId,
                    Text = Messages.NoOcassionsForStats,
                });
                return;
            }

            var answers = new List<PollAnswerEntity>();
            foreach (var pollEntity in polls)
            {
                var yesAnswers = _pollAnswerRepository.Find(x => x.PollId == pollEntity.RowKey && x.IsYesAnswer).ToList();
                answers.AddRange(yesAnswers);
            }

            var displayTable = await GetDisplayTableAsync(answers, polls.Count, chatId);
            await _telegramClient.SendMessageAsync(new SendMessage
            {
                ChatId = chatId,
                Text = displayTable,
                ParseMode = "HTML"
            });
        }

        private async Task<string> GetDisplayTableAsync(IEnumerable<PollAnswerEntity> answers, int pollsCount, string chatId)
        {
            var answersByUser = answers.GroupBy(x => x.UserId)
                                       .ToDictionary(x => x.Key, y => y.Count())
                                       .OrderByDescending(x => x.Value);

            var count = 1;
            var displayTable = "<pre>";

            displayTable += string.Format(Messages.StatsTotalOccasions, pollsCount);
            displayTable += "\r\n------------------------------";

            foreach (var kvp in answersByUser)
            {
                var userInfo = await _telegramClient.GetChatMemberAsync(chatId, kvp.Key);
                if (userInfo == null)
                {
                    continue;
                }

                var userDisplay = $"{userInfo.User.FirstName} {userInfo.User.LastName}";
                userDisplay = userDisplay.Trim();
                var percentage = (int) (((double) kvp.Value / (double) pollsCount) * 100);

                displayTable += "\r\n" + string.Format(Messages.StatsForUser, count, kvp.Value, percentage, userDisplay);
                count++;
            }

            displayTable += "\r\n</pre>";
            return displayTable;
        }
    }
}