namespace WebApplication1.Interfaces.ChatBot
{
    public interface IAIService
    {
        Task<string> GenerateResponseAsync(string prompt, string conversationHistory);
        Task<T> ExecuteToolAsync<T>(string toolName, object parameters);
    }
}
