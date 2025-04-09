using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class ListingsRepository:GenericRepository<Listing>
    {
        private readonly AirbnbDBContext context;
        private readonly IMapper mapper;

        public ListingsRepository(AirbnbDBContext _context, IMapper _mapper):base(_context,_mapper)
        {
            context = _context;
            mapper = _mapper;
        }
        public async Task<List<Listing>> GetListingsByHostAsync(Guid hostId)
        {
            return await context.Set<Listing>()
                                 .Where(l => l.HostId == hostId) 
                                 .ToListAsync(); 
        }

    }
}