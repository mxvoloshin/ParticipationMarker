using System;
using ParticipationMarker.Infrastrucutre;
using Telegram.API;

namespace ParticipationMarker.FunctionApp
{
    public class AppSettings : ICloudStorageSettings, ITelegramSettings
    {
        private string _connectionString;
        private string _botKey;

        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = Environment.GetEnvironmentVariable("AzureStorageConnectionString", EnvironmentVariableTarget.Process);
                }

                return _connectionString;
            }
        }

        public string BotKey
        {
            get
            {
                if (string.IsNullOrEmpty(_botKey))
                {
                    _botKey = Environment.GetEnvironmentVariable("TelegramBotKey", EnvironmentVariableTarget.Process);
                }

                return _botKey;
            }
        }
    }
}