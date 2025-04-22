// Controllers/ChatController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Interfaces.ChatBot;
using WebApplication1.Models.ChatBot;

namespace AirbnbClone.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatRepository _chatService;

        public ChatController(IChatRepository chatService)
        {
            _chatService = chatService;
        }

        //[HttpGet("test")]
        //[AllowAnonymous] // Skip authentication for testing
        //public async Task<IActionResult> TestChat()
        //{
        //    var userId = "test-user"; // Use any test user ID

        //    // Create a conversation
        //    var conversationId = await _chatService.CreateNewConversationAsync(userId);

        //    // Send a test message
        //    var response1 = await _chatService.ProcessMessageAsync(userId, "Add Beach House to my wishlist", conversationId);

        //    // Send another message
        //    var response2 = await _chatService.ProcessMessageAsync(userId, "Search for listings in Paris", conversationId);

        //    // Get history
        //    var history = await _chatService.GetConversationHistoryAsync(userId, conversationId);

        //    return Ok(new
        //    {
        //        conversationId,
        //        messages = history
        //    });
        //}

        [HttpGet("conversation/{conversationId}")]
        public async Task<ActionResult<List<ChatMessage>>> GetConversation(string conversationId)
        {
            var userId = User.Identity.Name;
            var messages = await _chatService.GetConversationHistoryAsync(userId, conversationId);
            return Ok(messages);
        }

        [HttpPost("send")]
        public async Task<ActionResult<ChatMessage>> SendMessage([FromBody] SendMessageRequest request)
        {
            var userId = User.Identity.Name;
            var response = await _chatService.ProcessMessageAsync(userId, request.Message, request.ConversationId);
            return Ok(response);
        }

        [HttpPost("conversation")]
        public async Task<ActionResult<string>> CreateConversation()
        {
            var userId = User.Identity.Name;
            var conversationId = await _chatService.CreateNewConversationAsync(userId);
            return Ok(new { conversationId });
        }
    }

    public class SendMessageRequest
    {
        public string Message { get; set; }
        public string ConversationId { get; set; }
    }
}