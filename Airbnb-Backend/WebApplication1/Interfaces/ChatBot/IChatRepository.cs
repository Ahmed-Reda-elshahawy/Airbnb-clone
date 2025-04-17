using WebApplication1.Models.ChatBot;

namespace WebApplication1.Interfaces.ChatBot
{
    public interface IChatService
    {
        Task<ChatMessage> ProcessMessageAsync(string userId, string message, string conversationId);
        Task<List<ChatMessage>> GetConversationHistoryAsync(string userId, string conversationId);
        Task<string> CreateNewConversationAsync(string userId);
    }
}
