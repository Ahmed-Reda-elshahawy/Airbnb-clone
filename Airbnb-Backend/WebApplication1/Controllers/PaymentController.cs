using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Stripe;
using WebApplication1.DTOS.Amenity;
using WebApplication1.DTOS.Payment;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Models.Enums;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        #region Dependency Injection
        private readonly IStripe _stripeRepository;
        private readonly IPayment _paymentRepository;
        
        public PaymentController(IStripe stripeRepository, IPayment paymentRepository)
        {
            _stripeRepository = stripeRepository;
            _paymentRepository = paymentRepository;
        }
        #endregion

        #region Create Payment Intent
        [HttpPost("booking/{bookingId}/create-intent")]
        public async Task<IActionResult> CreatePaymentIntent(Guid bookingId,[FromBody] PaymentIntentRequestDTO request)
        {
            try
            {
                var intent = await _stripeRepository.CreatePaymentIntentAsync(bookingId, request);
                return Ok(intent);
            }
            catch (StripeException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        #endregion

        #region Confirm Payment / Create
        [HttpPost("booking/{bookingId}/confirm")]
        public async Task<ActionResult<PaymentResponseDTO>> ConfirmPayment(Guid bookingId,[FromBody] ConfirmPaymentDTO dto)
        {
            try
            {
                var (intent, charge) = await _stripeRepository.ConfirmPaymentStripeAsync(bookingId, dto);
                var combinedData = (intent, charge, dto);
                var savedPayment = await _paymentRepository.CreatePaymentAsync(combinedData);
                if (Enum.TryParse<PaymentStatus>(savedPayment.Status, true, out var status) && status == PaymentStatus.Completed)
                {
                    await _paymentRepository.HandlePostPaymentSuccess(bookingId);
                }
                return Ok(savedPayment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        #endregion

        #region Cancel Payment Intent
        [HttpPost("cancel-intent/{paymentIntentId}")]
        public async Task<IActionResult> CancelPaymentIntent(string paymentIntentId)
        {
            try
            {
                await _stripeRepository.CancelPaymentIntentAsync(paymentIntentId);
                return Ok(new { message = "Payment intent cancelled successfully." });
            }
            catch (StripeException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        #endregion

        #region Get Methods
        [HttpGet]
        public async Task<IActionResult> GetAllPayments([FromQuery] Dictionary<string, string> queryParams)
        {
            var payments = await _paymentRepository.GetAllAsync(queryParams);
            return Ok(payments);
        }
        [HttpGet("me")]
        public async Task<IActionResult> GetUserPayments()
        {
            var userId = _paymentRepository.GetCurrentUserId();
            var payments = await _paymentRepository.GetAllAsync(new Dictionary<string, string> { { "UserId", userId.ToString() } });
            return Ok(payments);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserPaymentById(Guid id)
        {
            var payment = await _paymentRepository.GetByIDAsync(id);
            if (payment == null)
                return NotFound();
            return Ok(payment);
        }
        #endregion
    }
}
