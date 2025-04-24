using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOS.Statistics;
using WebApplication1.Interfaces;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        #region Dependency Injection
        private readonly IMapper _mapper;
        private readonly IListing _listingsRepository;
        private readonly IBooking _bookingRepository;
        private readonly IStatistics _statisticsRepository;

        public StatisticsController(IMapper mapper, IListing listingsRepository, IBooking bookingRepository,IStatistics statisticsRepository)
        {
            _mapper = mapper;
            _listingsRepository = listingsRepository;
            _bookingRepository = bookingRepository;
            _statisticsRepository = statisticsRepository;
        }
        #endregion
        #region Booking Statistics
        [HttpGet("bookings-per-month")]
        public async Task<IActionResult> GetBookingsPerMonth()
        {
            var currentYear = DateTime.Now.Year;

            (var bookingsPerMonth, var CancellationPerMonth) = await _statisticsRepository.GetBookingsPerMonth(currentYear);

            var labels = new List<string>
            {
                "January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            };
            var response = new BookingPerMonthDTO
            {
                Labels = labels,
                NewBookingsData = bookingsPerMonth,
                CancellationsData = CancellationPerMonth,
            };
            return Ok(response);
        }
        #endregion
        #region Monthly Revenue
        [HttpGet("monthly-revenue")]
        public async Task<IActionResult> GetMonthlyRevenue()
        {
            var currentYear = DateTime.Now.Year;

            var revenuePerMonth = await _statisticsRepository.GetMonthlyRevenueAsync(currentYear);

            var months = new List<string>
            {
                "January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            };
            var response = new RevenueTrendDTO
            {
                Months = months,
                Revenue = revenuePerMonth
            };

            return Ok(response);
        }
        #endregion
        #region User Role Distribution
        [HttpGet("role-distribution")]
        public async Task<IActionResult> GetRoleDistribution()
        {
            var data = await _statisticsRepository.GetRoleDistributionAsync();
            return Ok(data);
        }
        #endregion
        [HttpGet("top-hosts")]
        public async Task<IActionResult> GetTopHosts([FromQuery] int topN = 5)
        {
            var data = await _statisticsRepository.GetTopHostsAsync(topN);
            return Ok(data);
        }

    }
}
