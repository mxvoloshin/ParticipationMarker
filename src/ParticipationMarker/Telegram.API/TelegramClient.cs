using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telegram.API.Domain;

namespace Telegram.API
{
    public class TelegramClient : ITelegramClient
    {
        private readonly HttpClient _httpClient;
        private readonly ITelegramSettings _settings;
        private readonly ILogger<TelegramClient> _logger;
        private readonly JsonSerializerSettings _ignoreNullSerializer;

        public TelegramClient(HttpClient httpClient, ITelegramSettings settings, ILogger<TelegramClient> logger)
        {
            _httpClient = httpClient;
            _settings = settings;
            _logger = logger;
            _ignoreNullSerializer = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public async Task<Message> SendMessageAsync(SendMessage message)
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(message, _ignoreNullSerializer),
                Encoding.UTF8,
                "application/json");
            var responseMessage =
                await _httpClient.PostAsync($"{_httpClient.BaseAddress}{_settings.BotKey}/sendMessage", httpContent);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
            {
                // todo: how to handle errors?
                _logger.LogError($"Failed {responseMessage.StatusCode} - {responseContent}");
            }

            var response = JsonConvert.DeserializeObject<ResponseMessage>(responseContent);
            return response.Result;
        }

        public async Task<Message> SendPollAsync(SendPollMessage message)
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(message, _ignoreNullSerializer),
                Encoding.UTF8,
                "application/json");
            var responseMessage =
                await _httpClient.PostAsync($"{_httpClient.BaseAddress}{_settings.BotKey}/sendPoll", httpContent);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
            {
                // todo: how to handle errors?
                _logger.LogError($"Failed {responseMessage.StatusCode} - {responseContent}");
            }

            var response = JsonConvert.DeserializeObject<ResponseMessage>(responseContent);
            return response.Result;
        }

        public async Task StopPollAsync(StopPollMessage message)
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(message, _ignoreNullSerializer),
                Encoding.UTF8,
                "application/json");
            var responseMessage =
                await _httpClient.PostAsync($"{_httpClient.BaseAddress}{_settings.BotKey}/stopPoll", httpContent);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
            {
                // todo: how to handle errors?
                _logger.LogError($"Failed {responseMessage.StatusCode} - {responseContent}");
            }
        }

        public async Task<ChatMember> GetChatMemberAsync(string chatId, string userId)
        {
            dynamic getChatmember = new JObject();
            getChatmember.chat_id = chatId;
            getChatmember.user_id = userId;

            var httpContent = new StringContent(JsonConvert.SerializeObject(getChatmember, _ignoreNullSerializer),
                Encoding.UTF8,
                "application/json");
            var responseMessage =
                await _httpClient.PostAsync($"{_httpClient.BaseAddress}{_settings.BotKey}/getChatMember",
                    httpContent);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
            {
                // todo: how to handle errors?
                _logger.LogError($"Failed {responseMessage.StatusCode} - {responseContent}");
            }

            var response = JsonConvert.DeserializeObject<ResponseChatMember>(responseContent);
            return response.Ok ? response.Result : null;
        }

        public async Task<bool> IsAdminAsync(string chatId, string userId)
        {
            var admins = await GetChatAdministratorsAsync(chatId);
            return admins != null && admins.Any(x =>
                string.Equals(x.User.Id.ToString(), userId, StringComparison.OrdinalIgnoreCase));
        }

        public async Task AnswerCallbackQuery(AnswerCallbackQuery answerCallbackQuery)
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(answerCallbackQuery, _ignoreNullSerializer),
                Encoding.UTF8,
                "application/json");
            var responseMessage =
                await _httpClient.PostAsync($"{_httpClient.BaseAddress}{_settings.BotKey}/answerCallbackQuery",
                    httpContent);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
            {
                // todo: how to handle errors?
                _logger.LogError($"Failed {responseMessage.StatusCode} - {responseContent}");
            }
        }

        public async Task EditMessageText(EditMessageText message)
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(message, _ignoreNullSerializer),
                Encoding.UTF8,
                "application/json");
            var responseMessage =
                await _httpClient.PostAsync($"{_httpClient.BaseAddress}{_settings.BotKey}/editMessageText", httpContent);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
            {
                // todo: how to handle errors?
                _logger.LogError($"Failed {responseMessage.StatusCode} - {responseContent}");
            }
        }

        private async Task<List<ChatMember>> GetChatAdministratorsAsync(string chatId)
        {
            dynamic getChatAdministrators = new JObject();
            getChatAdministrators.chat_id = chatId;

            var httpContent = new StringContent(JsonConvert.SerializeObject(getChatAdministrators, _ignoreNullSerializer),
                Encoding.UTF8,
                "application/json");
            var responseMessage =
                await _httpClient.PostAsync($"{_httpClient.BaseAddress}{_settings.BotKey}/getChatAdministrators",
                    httpContent);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
            {
                // todo: how to handle errors?
                _logger.LogError($"Failed {responseMessage.StatusCode} - {responseContent}");
            }

            var response = JsonConvert.DeserializeObject<ResponseChatMembers>(responseContent);
            return response.Ok ? response.Result : null;
        }
    }
}