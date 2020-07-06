using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ParticipationMarker.App.Services.Occasion;
using ParticipationMarker.App.Services.Poll;
using ParticipationMarker.App.Services.Stats;
using ParticipationMarker.App.TelegramBot;
using ParticipationMarker.Common.Services;
using ParticipationMarker.FunctionApp;
using ParticipationMarker.Infrastrucutre;
using ParticipationMarker.Infrastrucutre.Occasion;
using ParticipationMarker.Infrastrucutre.Poll;
using ParticipationMarker.Infrastrucutre.PollAnswer;
using Telegram.API;

[assembly: FunctionsStartup(typeof(Startup))]

namespace ParticipationMarker.FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient<ITelegramClient, TelegramClient>(client => client.BaseAddress = new Uri("https://api.telegram.org"));

            builder.Services.AddSingleton<ITelegramInputParser, TelegramInputParser>();
            builder.Services.AddSingleton<ICloudStorageSettings, AppSettings>();
            builder.Services.AddSingleton<ITelegramSettings, AppSettings>();

            builder.Services.AddTransient<IOccasionRepository, OccasionRepository>();
            builder.Services.AddTransient<IPollRepository, PollRepository>();
            builder.Services.AddTransient<IPollAnswerRepository, PollAnswerRepository>();
            builder.Services.AddTransient<IPollRepository, PollRepository>();

            builder.Services.AddTransient<IDateTimeService, DateTimeService>();
            builder.Services.AddTransient<IPollService, PollService>();
            builder.Services.AddTransient<IOccasionService, OccasionService>();
            builder.Services.AddTransient<IPollService, PollService>();
            builder.Services.AddTransient<IStatsService, StatsService>();
        }
    }
}