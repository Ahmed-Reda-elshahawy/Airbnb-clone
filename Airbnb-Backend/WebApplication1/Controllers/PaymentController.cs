using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBookingRepository _bookingRepository;

        public PaymentController(IPaymentRepository paymentRepository, IBookingRepository bookingRepository)
        {
            _paymentRepository = paymentRepository;
            _bookingRepository = bookingRepository;
        }

        // POST: api/payments/booking/{bookingId}
        [HttpPost("booking/{bookingId}")]
        public IActionResult ProcessPayment(Guid bookingId, [FromBody] PaymentProcessRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = Guid.Parse(User.FindFirst("sub")?.Value);

            var booking = _bookingRepository.GetBookingDetails(bookingId);
            if (booking == null)
                return NotFound("Booking not found");

            if (booking.GuestId != userId)
                return Forbid();

            var success = _paymentRepository.ProcessPayment(
                bookingId,
                userId,
                request.PaymentMethodId,
                request.Amount);

            if (!success)
                return BadRequest("Payment processing failed");

            return Ok(new { Message = "Payment processed successfully" });
        }

        // GET: api/payments/me
        [HttpGet("me")]
        public IActionResult GetUserPayments()
        {
            var userId = Guid.Parse(User.FindFirst("sub")?.Value);
            var payments = _paymentRepository.GetUserPayments(userId);
            return Ok(payments);
        }

        // GET: api/payments/{id}
        [HttpGet("{id}")]
        public IActionResult GetPaymentDetails(Guid id)
        {
            var payment = _paymentRepository.GetById(id);
            if (payment == null)
                return NotFound();

            var userId = Guid.Parse(User.FindFirst("sub")?.Value);
            if (payment.UserId != userId)
                return Forbid();

            return Ok(payment);
        }

        // POST: api/payments/methods
        [HttpPost("methods")]
        public IActionResult AddPaymentMethod([FromBody] PaymentMethod method)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = Guid.Parse(User.FindFirst("sub")?.Value);

            var success = _paymentRepository.AddPaymentMethod(userId, method);
            if (!success)
                return BadRequest("Failed to add payment method");

            return CreatedAtAction(nameof(GetUserPaymentMethods), new { id = method.Id }, method);
        }

        // GET: api/payments/methods
        [HttpGet("methods")]
        public IActionResult GetUserPaymentMethods()
        {
            var userId = Guid.Parse(User.FindFirst("sub")?.Value);
            var methods = _paymentRepository.GetUserPaymentMethods(userId);
            return Ok(methods);
        }
    }

    public class PaymentProcessRequest
    {
        public int PaymentMethodId { get; set; }
        public decimal Amount { get; set; }
    }
}

