using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOS.AvailabilityCalendar;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class AvailabilityCalendarRepository : GenericRepository<AvailabilityCalendar>
    {
        #region Dependency Injection
        private readonly AirbnbDBContext context;
        private readonly IMapper mapper;
        public AvailabilityCalendarRepository(AirbnbDBContext _context, IMapper _mapper) : base(_context, _mapper)
        {
            context = _context;
            mapper = _mapper;
        }
        #endregion

        #region Initialize Availability
        public async Task<int> InitializeAvailabilityAsync(Guid listingId, InitAvailabilityCalendarDTO dto)
        {
            var listingExists = await context.Listings.AnyAsync(l => l.Id == listingId);
            if (!listingExists)
            {
                throw new Exception("Listing not found.");
            }

            var startDate = DateTime.UtcNow.Date;
            var endDate = startDate.AddMonths(dto.MonthsAhead);

            var existingDates = await context.AvailabilityCalendars
                .Where(a => a.ListingId == listingId && a.Date >= startDate && a.Date <= endDate)
                .Select(a => a.Date)
                .ToListAsync();

            var datesToAdd = new List<AvailabilityCalendar>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (existingDates.Contains(date)) continue;

                var day = mapper.Map<AvailabilityCalendar>(dto);
                day.ListingId = listingId;
                day.Date = date;

                datesToAdd.Add(day);
            }

            if (datesToAdd.Count != 0)
            {
                await context.AvailabilityCalendars.AddRangeAsync(datesToAdd);
                await context.SaveChangesAsync();
            }

            return datesToAdd.Count;
        }
        #endregion

        #region Set Availability With Start and End Date
        public async Task<int> SetAvailabilityRange (Guid listingId, SetAvailabilityCalendarDTO dto)
        {
            var listingExists = await context.Listings.AnyAsync(l => l.Id == listingId);
            if (!listingExists)
            {
                throw new Exception("Listing not found.");
            }

            var dates = Enumerable.Range(0, (dto.EndDate - dto.StartDate).Days + 1)
                .Select(offset => dto.StartDate.AddDays(offset))
                .ToList();

            var availabilityEntries = dates.Select(date =>
            {
                var availabilityCalendar = mapper.Map<AvailabilityCalendar>(dto);
                availabilityCalendar.ListingId = listingId;
                availabilityCalendar.Date = date;
                return availabilityCalendar;
            }).ToList();

            await context.AvailabilityCalendars.AddRangeAsync(availabilityEntries);
            await context.SaveChangesAsync();

            return availabilityEntries.Count;
        }
        #endregion
    }
}