using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        #region Stripe

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

        #region Confirm Payment

        [HttpPost("booking/{bookingId}/confirm")]
        public async Task<ActionResult<PaymentResponseDTO>> ConfirmPayment(Guid bookingId,[FromBody] ConfirmPaymentDTO dto)
        {
            try
            {
                var result = await _stripeRepository.ConfirmPaymentStripeAsync(bookingId, dto);
                var combinedData = (result.intent, result.charge, dto);
                var savedPayment = await _paymentRepository.CreatePaymentAsync(combinedData);
                if (savedPayment.Status == PaymentStatus.Completed.ToString())
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

        #endregion
        [HttpGet("payment-methods")]
        public async Task<ActionResult<IEnumerable<Models.PaymentMethod>>> GetPaymentMethods([FromQuery] Dictionary<string, string> queryParams)
        {
            try
            {
                var paymentMethods = await _paymentRepository.GetAllAsync(queryParams);
                return Ok(paymentMethods);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}


        //[HttpPost("webhook")]
        //public async Task<IActionResult> StripeWebhook()
        //{
        //    var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        //    try
        //    {
        //        var stripeEvent = EventUtility.ConstructEvent(
        //            json,
        //            Request.Headers["Stripe-Signature"],
        //            "your_webhook_secret" // هتاخديه من Stripe Dashboard
        //        );

        //        if (stripeEvent.Type == Events.PaymentIntentSucceeded)
        //        {
        //            var intent = stripeEvent.Data.Object as PaymentIntent;

        //            // TODO: سجلي الدفع في قاعدة البيانات
        //            // ممكن تستخدمي intent.Id كـ TransactionId
        //        }

        //        return Ok();
        //    }
        //    catch (StripeException e)
        //    {
        //        return BadRequest();
        //    }
        //}

    




    //    // POST: api/payments/booking/{bookingId}
    //    [HttpPost("booking/{bookingId}")]
    //    public IActionResult ProcessPayment(Guid bookingId, [FromBody] PaymentProcessRequest request)
    //    {
    //        if (!ModelState.IsValid)
    //            return BadRequest(ModelState);

    //        var userId = Guid.Parse(User.FindFirst("sub")?.Value);

    //        var booking = _bookingRepository.GetBookingDetails(bookingId);
    //        if (booking == null)
    //            return NotFound("Booking not found");

    //        if (booking.GuestId != userId)
    //            return Forbid();

    //        var success = _paymentRepository.ProcessPayment(
    //            bookingId,
    //            userId,
    //            request.PaymentMethodId,
    //            request.Amount);

    //        if (!success)
    //            return BadRequest("Payment processing failed");

    //        return Ok(new { Message = "Payment processed successfully" });
    //    }

    //    // GET: api/payments/me
    //    [HttpGet("me")]
    //    public IActionResult GetUserPayments()
    //    {
    //        var userId = Guid.Parse(User.FindFirst("sub")?.Value);
    //        var payments = _paymentRepository.GetUserPayments(userId);
    //        return Ok(payments);
    //    }

    //    // GET: api/payments/{id}
    //    [HttpGet("{id}")]
    //    public IActionResult GetPaymentDetails(Guid id)
    //    {
    //        var payment = _paymentRepository.GetById(id);
    //        if (payment == null)
    //            return NotFound();

    //        var userId = Guid.Parse(User.FindFirst("sub")?.Value);
    //        if (payment.UserId != userId)
    //            return Forbid();

    //        return Ok(payment);
    //    }

    //    // POST: api/payments/methods
    //    [HttpPost("methods")]
    //    public IActionResult AddPaymentMethod([FromBody] PaymentMethod method)
    //    {
    //        if (!ModelState.IsValid)
    //            return BadRequest(ModelState);

    //        var userId = Guid.Parse(User.FindFirst("sub")?.Value);

    //        var success = _paymentRepository.AddPaymentMethod(userId, method);
    //        if (!success)
    //            return BadRequest("Failed to add payment method");

    //        return CreatedAtAction(nameof(GetUserPaymentMethods), new { id = method.Id }, method);
    //    }

    //    // GET: api/payments/methods
    //    [HttpGet("methods")]
    //    public IActionResult GetUserPaymentMethods()
    //    {
    //        var userId = Guid.Parse(User.FindFirst("sub")?.Value);
    //        var methods = _paymentRepository.GetUserPaymentMethods(userId);
    //        return Ok(methods);
    //    }
