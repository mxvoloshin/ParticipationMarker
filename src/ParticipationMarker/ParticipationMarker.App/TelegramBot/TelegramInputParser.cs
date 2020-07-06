using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParticipationMarker.App.Enums;
using ParticipationMarker.App.Extensions;
using ParticipationMarker.App.Services.Occasion;
using ParticipationMarker.App.Services.Poll;
using ParticipationMarker.App.Services.Stats;
using Telegram.API.Domain;

namespace ParticipationMarker.App.TelegramBot
{
    public class TelegramInputParser : ITelegramInputParser
    {
        private readonly IOccasionService _occasionService;
        private readonly IPollService _pollService;
        private readonly IStatsService _statsService;

        public TelegramInputParser(IOccasionService occasionService,
                                   IPollService pollService,
                                   IStatsService statsService)
        {
            _occasionService = occasionService;
            _pollService = pollService;
            _statsService = statsService;
        }

        public async Task ParseRequestAsync(string requestBody)
        {
            var updateMessage = JsonConvert.DeserializeObject<UpdateMessage>(requestBody);

            var messageType = updateMessage.ParseMessageType();
            switch (messageType)
            {
                case UpdateMessageType.Unknown:
                    break;
                case UpdateMessageType.GetNewOccasionName:
                    await _occasionService.RequestNewOccasionNameAsync(updateMessage);
                    break;
                case UpdateMessageType.CreateOccasion:
                    await _occasionService.CreateAsync(updateMessage);
                    break;
                case UpdateMessageType.GetOccasionsForDelete:
                    await _occasionService.GetOccasionsForDeleteAsync(updateMessage);
                    break;
                case UpdateMessageType.DeleteOccasion:
                    await _occasionService.DeleteAsync(updateMessage);
                    break;
                case UpdateMessageType.GetOccasionsToStart:
                    await _occasionService.GetOccasionsToStartAsync(updateMessage);
                    break;
                case UpdateMessageType.StartPoll:
                    await _pollService.StartAsync(updateMessage);
                    break;
                case UpdateMessageType.StopActivePoll:
                    await _pollService.StopAllActivePollsAsync(updateMessage);
                    break;
                case UpdateMessageType.PollAnswered:
                    await _pollService.AddAnswerAsync(updateMessage);
                    break;
                case UpdateMessageType.Stats:
                    await _statsService.ShowAsync(updateMessage);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}