using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParticipationMarker.App.Domain;
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

            var (pollsCount, chatMemberStats) = await GetStatAsync(chatId);
            var displayTable = GetDisplayTable(pollsCount, chatMemberStats);
            await _telegramClient.SendMessageAsync(new SendMessage
            {
                ChatId = chatId,
                Text = displayTable,
                ParseMode = "HTML"
            });
        }

        public async Task<IEnumerable<ChatMemberStat>> GetChatStatAsync(string chatName)
        {
            var chatId = await GetChatIdAsync(chatName);
            if (string.IsNullOrEmpty(chatId))
            {
                return Enumerable.Empty<ChatMemberStat>();
            }

            var result = await GetStatAsync(chatId);
            return result.Item2;
        }

        private string GetDisplayTable(int pollsCount, IEnumerable<ChatMemberStat> memberStats)
        {
            var count = 1;
            var displayTable = "<pre>";

            displayTable += string.Format(Messages.StatsTotalOccasions, pollsCount);
            displayTable += "\r\n------------------------------";

            foreach (var memberStat in memberStats)
            {
                var userDisplay = $"{memberStat.FirstName} {memberStat.LastName}";
                userDisplay = userDisplay.Trim();

                displayTable += "\r\n" + string.Format(Messages.StatsForUser, count, memberStat.TotalVisited, memberStat.Percentage, userDisplay);
                count++;
            }

            displayTable += "\r\n</pre>";
            return displayTable;
        }

        private async Task<string> GetChatIdAsync(string chatName)
        {
            var chatIds = _occasionRepository.Find(x => x.ChatId != "").Select(x => x.PartitionKey).ToList().Distinct();

            foreach (var chatId in chatIds)
            {
                var chatInfo = await _telegramClient.GetChatAsync(chatId);
                if (chatInfo == null)
                {
                    continue;
                }

                if (string.Equals(chatInfo.Title, chatName, StringComparison.OrdinalIgnoreCase))
                {
                    return chatId;
                }
            }

            return string.Empty;
        }

        private async Task<Tuple<int, IEnumerable<ChatMemberStat>> > GetStatAsync(string chatId)
        {
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
                return new Tuple<int, IEnumerable<ChatMemberStat>>(0, Enumerable.Empty<ChatMemberStat>());
            }

            var answers = new List<PollAnswerEntity>();
            foreach (var pollEntity in polls)
            {
                var yesAnswers = _pollAnswerRepository.Find(x => x.PollId == pollEntity.RowKey && x.IsYesAnswer).ToList();
                answers.AddRange(yesAnswers);
            }

            var answersByUser = answers.GroupBy(x => x.UserId)
                                       .ToDictionary(x => x.Key, y => y.Count())
                                       .OrderByDescending(x => x.Value);

            var result = new List<ChatMemberStat>();
            foreach (var kvp in answersByUser)
            {
                var userInfo = await _telegramClient.GetChatMemberAsync(chatId, kvp.Key);
                if (userInfo == null)
                {
                    continue;
                }

                var percentage = (int) (((double) kvp.Value / (double) polls.Count) * 100);
                result.Add(new ChatMemberStat
                {
                    FirstName = userInfo.User.FirstName,
                    LastName = userInfo.User.LastName,
                    TotalVisited = kvp.Value,
                    Percentage = percentage
                });
            }

            return new Tuple<int, IEnumerable<ChatMemberStat>>(polls.Count, result);
        }
    }
}