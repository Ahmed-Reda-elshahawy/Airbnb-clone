//using System.Text.Json;
//using System.Text.RegularExpressions;
//using Microsoft.EntityFrameworkCore;
//using WebApplication1.Interfaces.ChatBot;
//using WebApplication1.Models;
//using WebApplication1.Models.ChatBot;

//namespace WebApplication1.Repositories.ChatBot
//{
//    public class ChatService : IChatService
//    {
//        private readonly IAIService _aiService;
//        private readonly AirbnbDBContext _context;
//        private readonly WishListRepository _wishlistService;
//        private readonly ListingsRepository _listingService;
//        private readonly BookingRepository _bookingService;
//        private readonly ReviewsRepository _reviewService;

//        public ChatService(
//            IAIService aiService,
//            AirbnbDBContext context,
//            WishListRepository wishlistService,
//            ListingsRepository listingService,
//            BookingRepository bookingService,
//            ReviewsRepository reviewService)
//        {
//            _aiService = aiService;
//            _context = context;
//            _wishlistService = wishlistService;
//            _listingService = listingService;
//            _bookingService = bookingService;
//            _reviewService = reviewService;
//        }

//        public async Task<string> CreateNewConversationAsync(string userId)
//        {
//            var conversationId = Guid.NewGuid().ToString();

//            // Create initial greeting message from assistant
//            var welcomeMessage = new ChatMessage
//            {
//                UserId = userId,
//                IsFromUser = false,
//                Content = "Hello! I'm your Airbnb assistant. I can help you search for listings, manage your wishlist, make bookings, and more. How can I assist you today?",
//                Timestamp = DateTime.UtcNow,
//                ConversationId = conversationId
//            };

//            _context.ChatMessages.Add(welcomeMessage);
//            await _context.SaveChangesAsync();

//            return conversationId;
//        }

//        public async Task<List<ChatMessage>> GetConversationHistoryAsync(string userId, string conversationId)
//        {
//            return await _context.ChatMessages
//                .Where(m => m.UserId == userId && m.ConversationId == conversationId)
//                .OrderBy(m => m.Timestamp)
//                .ToListAsync();
//        }

//        public async Task<ChatMessage> ProcessMessageAsync(string userId, string message, string conversationId)
//        {
//            // Save user message
//            var userMessage = new ChatMessage
//            {
//                UserId = userId,
//                IsFromUser = true,
//                Content = message,
//                Timestamp = DateTime.UtcNow,
//                ConversationId = conversationId
//            };

//            _context.ChatMessages.Add(userMessage);
//            await _context.SaveChangesAsync();

//            // Get conversation history for context
//            var history = await GetConversationHistoryAsync(userId, conversationId); // Fixed: Changed userId type to string

//            var conversationText = string.Join("\n", history.Select(m =>
//                m.IsFromUser ? $"User: {m.Content}" : $"Assistant: {m.Content}"));

//            // Process the message to determine intent
//            var intent = await DetermineIntentAsync(message, conversationText);
//            string responseContent;

//            try
//            {
//                switch (intent.Type)
//                {
//                    case "add_to_wishlist":
//                        responseContent = await HandleAddToWishlistAsync(Guid.Parse(userId), intent.Parameters);
//                        break;
//                    case "search_listings":
//                        responseContent = await HandleSearchListingsAsync(intent.Parameters);
//                        break;
//                    case "make_booking":
//                        responseContent = await HandleMakeBookingAsync(userId, intent.Parameters);
//                        break;
//                    case "add_review":
//                        responseContent = await HandleAddReviewAsync(userId, intent.Parameters);
//                        break;
//                    default:
//                        // For general conversation, use the AI service
//                        responseContent = await _aiService.GenerateResponseAsync(message, conversationText);
//                        break;
//                }
//            }
//            catch (Exception ex)
//            {
//                responseContent = $"I encountered an issue while processing your request: {ex.Message}. Could you try again with more details?";
//            }

//            // Save assistant response
//            var assistantMessage = new ChatMessage
//            {
//                UserId = userId,
//                IsFromUser = false,
//                Content = responseContent,
//                Timestamp = DateTime.UtcNow,
//                ConversationId = conversationId
//            };

//            _context.ChatMessages.Add(assistantMessage);
//            await _context.SaveChangesAsync();

//            return assistantMessage;
//        }

//        private async Task<(string Type, Dictionary<string, object> Parameters)> DetermineIntentAsync(string message, string conversationHistory)
//        {
//            // Use the AI service to determine the intent
//            var prompt = $"Analyze the following message and determine the user's intent. " +
//                         $"Respond with a JSON object containing 'type' (one of: add_to_wishlist, search_listings, make_booking, add_review, general_question) " +
//                         $"and 'parameters' as an object with relevant parameters for the action. Message: \"{message}\"";

//            var response = await _aiService.GenerateResponseAsync(prompt, "");

//            try
//            {
//                // Try to extract JSON using regex in case the model adds extra text
//                var jsonMatch = Regex.Match(response, @"\{.*\}", RegexOptions.Singleline);
//                if (jsonMatch.Success)
//                {
//                    var jsonStr = jsonMatch.Value;
//                    var intentObj = JsonSerializer.Deserialize<JsonElement>(jsonStr);

//                    var type = intentObj.GetProperty("type").GetString() ?? "general_question";
//                    var parameters = new Dictionary<string, object>();

//                    if (intentObj.TryGetProperty("parameters", out var paramsElement) &&
//                        paramsElement.ValueKind == JsonValueKind.Object)
//                    {
//                        foreach (var param in paramsElement.EnumerateObject())
//                        {
//                            parameters[param.Name] = param.Value.ValueKind switch
//                            {
//                                JsonValueKind.String => param.Value.GetString(),
//                                JsonValueKind.Number => param.Value.GetInt32(),
//                                JsonValueKind.True => true,
//                                JsonValueKind.False => false,
//                                _ => null
//                            };
//                        }
//                    }

//                    return (type, parameters);
//                }
//            }
//            catch
//            {
//                // If JSON parsing fails, default to general question
//            }

//            return ("general_question", new Dictionary<string, object>());
//        }

//        private async Task<string> HandleAddToWishlistAsync(Guid userId, Dictionary<string, object> parameters)
//        {
//            // Convert parameters to Dictionary<string, string>
//            var stringParameters = parameters.ToDictionary(
//                kvp => kvp.Key,
//                kvp => kvp.Value?.ToString() ?? string.Empty
//            );

//            if (!stringParameters.TryGetValue("listingName", out var listingName) || string.IsNullOrEmpty(listingName))
//            {
//                return "I couldn't determine which listing you want to add to your wishlist. Could you specify the listing name?";
//            }

//            var listing = await _listingService.GetListingsWithDetails(stringParameters);
//            var firstListing = listing.FirstOrDefault();
//            if (listing == null)
//            {
//                return $"I couldn't find a listing called '{listingName}'. Please check the name and try again.";
//            }

//            // Check if user has a default wishlist or create one
//            var wishlist = await _wishlistService.GetUserWishlistsAsync(userId);
//            if (wishlist == null)
//            {
//                wishlist = await _wishlistService.CreateWishlistAsync(userId);
//            }

//            // Add to wishlist
//            await _wishlistService.AddItemToWishlistAsync(userId, firstListing.Id);

//            return $"Great! I've added '{firstListing.Title}' to your wishlist.";
//        }

//        private async Task<string> HandleSearchListingsAsync(Dictionary<string, object> parameters)
//        {
//            // Extract search parameters
//            var searchCriteria = new Dictionary<string, string>();
//            foreach (var param in parameters)
//            {
//                if (param.Value != null)
//                {
//                    searchCriteria[param.Key] = param.Value.ToString();
//                }
//            }
//            // Perform search
//            var listings = await _listingService.GetListingsWithDetails(searchCriteria);

//            if (listings == null || !listings.Any())
//            {
//                return "I couldn't find any listings matching your criteria. Try adjusting your search parameters.";
//            }

//            // Format results
//            var resultText = $"I found {listings.Count()} listings matching your criteria:\n\n";
//            foreach (var listing in listings.Take(5))
//            {
//                resultText += $"• {listing.Title} - {listing.PricePerNight:C} per night - {listing.Country}\n";
//            }

//            if (listings.Count() > 5)
//            {
//                resultText += $"\nAnd {listings.Count() - 5} more results. You can view all results on the search page.";
//            }

//            return resultText;
//        }

//        private async Task<string> HandleMakeBookingAsync(string userId, Dictionary<string, object> parameters)
//        {
            
//            // Extract booking parameters
//            if (!parameters.TryGetValue("listingName", out var listingNameObj) || listingNameObj is not string listingName)
//            {
//                return "I need the name of the listing you want to book. Please specify the listing name.";
//            }

//            // Get default values for missing parameters
//            int guests = 1;
//            int nights = 1;
//            DateTime checkIn = DateTime.Now.AddDays(1);

//            if (parameters.TryGetValue("guests", out var guestsObj) && guestsObj is int guestsVal)
//            {
//                guests = guestsVal;
//            }

//            if (parameters.TryGetValue("nights", out var nightsObj) && nightsObj is int nightsVal)
//            {
//                nights = nightsVal;
//            }

//            // Find the listing
//            var searchCriteria = new Dictionary<string, string>();
//            foreach (var param in parameters)
//            {
//                if (param.Value != null)
//                {
//                    searchCriteria[param.Key] = param.Value.ToString();
//                }
//            }
//            var listing = await _listingService.GetListingsWithDetails(searchCriteria);
//            if (listing == null)
//            {
//                return $"I couldn't find a listing called '{listingName}'. Please check the name and try again.";
//            }
//            var firstListing = listing.FirstOrDefault();
//            // Create booking
//            var bookingId = await _bookingService.CreateBooking(
//                userId,
//                firstListing.Id,
//                checkIn,
//                checkIn.AddDays(nights),
//                guests);

//            public Guid ListingId { get; set; }
//        public DateTime CheckInDate { get; set; }
//        public DateTime CheckOutDate { get; set; }
//        public int GuestsCount { get; set; }
//        public string SpecialRequests { get; set; }

//            return $"I've started a booking for '{listing.Name}' for {guests} guest(s) for {nights} night(s) starting {checkIn:d}. " +
//                   $"The total price would be {listing.Price * nights:C}. You can proceed to payment from your bookings page.";
//        }

//        private async Task<string> HandleAddReviewAsync(string userId, Dictionary<string, object> parameters)
//        {
//            // Extract review parameters
//            if (!parameters.TryGetValue("listingName", out var listingNameObj) || listingNameObj is not string listingName)
//            {
//                return "I need the name of the listing you want to review. Please specify the listing name.";
//            }

//            if (!parameters.TryGetValue("rating", out var ratingObj) || ratingObj is not int rating)
//            {
//                return "Please specify a rating between 1 and 5 stars.";
//            }

//            if (!parameters.TryGetValue("comment", out var commentObj) || commentObj is not string comment)
//            {
//                return "Please provide a comment for your review.";
//            }

//            // Find the listing
//            var listing = await _listingService.GetListingByNameAsync(listingName.ToString());
//            if (listing == null)
//            {
//                return $"I couldn't find a listing called '{listingName}'. Please check the name and try again.";
//            }

//            // Check if user has booked this listing before (optional step)
//            var hasBooked = await _bookingService.HasUserBookedListingAsync(userId, listing.Id);
//            if (!hasBooked)
//            {
//                return $"It looks like you haven't stayed at '{listing.Name}' yet. You can only review listings after your stay.";
//            }

//            // Add review
//            await _reviewService.AddReviewAsync(userId, listing.Id, rating, comment);

//            return $"Thank you! Your {rating}-star review for '{listing.Name}' has been submitted.";
//        }
//    }
//}
