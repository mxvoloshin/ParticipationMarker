using System.Threading.Tasks;
using Telegram.API.Domain;

namespace Telegram.API
{
    public interface ITelegramClient
    {
        Task<Message> SendMessageAsync(SendMessage message);
        Task<Message> SendPollAsync(SendPollMessage message);
        Task StopPollAsync(StopPollMessage message);
        Task<ChatMember> GetChatMemberAsync(string chatId, string userId);
        Task<bool> IsAdminAsync(string chatId, string userId);
        Task AnswerCallbackQuery(AnswerCallbackQuery answerCallbackQuery);
        Task EditMessageText(EditMessageText message);
        Task<Chat> GetChatAsync(string chatId);
    }
}