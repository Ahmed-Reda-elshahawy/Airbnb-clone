using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOS.Statistics;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Models.Enums;

namespace WebApplication1.Repositories
{
    public class StatisticsRepository :  IStatistics
    {
        #region Dependency Injection
        private readonly AirbnbDBContext context;
        private readonly IMapper mapper;
        private readonly IBooking bookingRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        public StatisticsRepository(AirbnbDBContext _context, IMapper _mapper, IBooking _bookingRepository, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole<Guid>> _roleManager) 
        {
            context = _context;
            mapper = _mapper;
            bookingRepository = _bookingRepository;
            userManager = _userManager;
            roleManager = _roleManager;
        }
        #endregion
        #region Booking Statistics
        public async Task<(List<int>,List<int>)> GetBookingsPerMonth(int year)
        {
            var bookingsPerMonth = new List<int>();
            var CancellationPerMonth = new List<int>();
            for (int month = 1; month <= 12; month++)
            {
                var bookingCount = await context.Bookings
                    .Where(b => b.CheckInDate.Year == year && b.CheckInDate.Month == month && b.Status == BookingStatus.Confirmed)
                    .CountAsync();
                var CancellationCount = await context.Bookings
                    .Where(b => b.Status == BookingStatus.Cancelled && b.CheckInDate.Year == year && b.CheckInDate.Month == month)
                    .CountAsync();
                bookingsPerMonth.Add(bookingCount);
                CancellationPerMonth.Add(CancellationCount);
            }
            return (bookingsPerMonth,CancellationPerMonth);

        }
        #endregion
        #region Revenue Statistics
        public async Task<List<decimal>> GetMonthlyRevenueAsync(int year)
        {
            var revenuePerMonth = new List<decimal>();

            for (int month = 1; month <= 12; month++)
            {
                var totalRevenue = await context.Bookings
                    .Where(b => b.CheckInDate.Year == year && b.CheckInDate.Month == month&& b.Status == BookingStatus.Confirmed)
                    .SumAsync(b => b.TotalPrice);

                revenuePerMonth.Add(totalRevenue);
            }

            return revenuePerMonth;
        }
        #endregion
        #region User Role Distribution
        public async Task<UserRoleDitributionDTO> GetRoleDistributionAsync()
        {
            var result = new UserRoleDitributionDTO();
            var allRoles = roleManager.Roles.Select(r => r.Name).ToList();
            foreach (var role in allRoles)
            {
                var usersInRole = await userManager.GetUsersInRoleAsync(role);
                result.Roles.Add(role);
                result.Counts.Add(usersInRole.Count);
            }
            return result;
        }
        #endregion
        public async Task<TopHostsDTO> GetTopHostsAsync(int topN = 5)
        {
            var result = await context.Bookings
                .Include(b => b.Listing)
                .ThenInclude(l => l.Host)
                .Where(b => b.Status == BookingStatus.Confirmed)
                .GroupBy(b => new { b.Listing.Host.Id, b.Listing.Host.FirstName, b.Listing.Host.LastName })
                .Select(g => new
                {
                    FullName = g.Key.FirstName + " " + g.Key.LastName,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(topN)
                .ToListAsync();

            return new TopHostsDTO
            {
                Hosts = result.Select(r => r.FullName).ToList(),
                BookingCounts = result.Select(r => r.Count).ToList()
            };
        }
    }
}
