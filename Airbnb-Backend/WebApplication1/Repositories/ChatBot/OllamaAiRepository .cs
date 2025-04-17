using System.Text;
using System.Text.Json;
using WebApplication1.Interfaces.ChatBot;

namespace WebApplication1.Repositories.ChatBot
{
    public class OllamaAIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _modelName;
        private readonly IConfiguration _configuration;

        public OllamaAIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _modelName = _configuration["AI:ModelName"] ?? "llama3";

            // Set base address for local Ollama instance
            _httpClient.BaseAddress = new Uri(_configuration["AI:EndpointUrl"] ?? "http://localhost:11434");
        }

        public async Task<string> GenerateResponseAsync(string prompt, string conversationHistory)
        {
            var requestBody = new
            {
                model = _modelName,
                prompt = $"{conversationHistory}\nUser: {prompt}\nAssistant:",
                stream = false,
                options = new
                {
                    temperature = 0.7,
                    num_predict = 1000
                }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("/api/generate", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<OllamaResponse>(responseString);

            return responseData?.Response ?? "I couldn't generate a response. Please try again.";
        }

        public async Task<T> ExecuteToolAsync<T>(string toolName, object parameters)
        {
            // This would implement tool execution logic based on the identified action
            // For now, we'll return default since the actual execution will happen in the ChatService
            return default;
        }

        private class OllamaResponse
        {
            public string Response { get; set; }
        }
    }
}
