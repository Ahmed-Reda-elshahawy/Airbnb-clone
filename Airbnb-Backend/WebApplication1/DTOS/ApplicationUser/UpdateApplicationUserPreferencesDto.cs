using WebApplication1.Models;

namespace WebApplication1.DTOS.ApplicationUser
{
    public class UpdateApplicationUserPreferencesDto
    {
        public string PreferredLanguage { get; set; }

        public int? CurrencyId { get; set; }
    }
}
