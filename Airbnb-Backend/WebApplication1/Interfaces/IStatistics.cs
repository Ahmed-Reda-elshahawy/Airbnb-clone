using WebApplication1.DTOS.Statistics;

namespace WebApplication1.Interfaces
{
    public interface IStatistics
    {
        Task<(List<int>, List<int>)> GetBookingsPerMonth(int year);
        Task<List<decimal>> GetMonthlyRevenueAsync(int year);
        Task<UserRoleDitributionDTO> GetRoleDistributionAsync();
        Task<TopHostsDTO> GetTopHostsAsync(int topN);
    }
}
