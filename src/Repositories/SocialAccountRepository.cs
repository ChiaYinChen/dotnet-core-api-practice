using Microsoft.EntityFrameworkCore;
using WebApiApp.Data;
using WebApiApp.Entities;

namespace WebApiApp.Repositories
{
    public class SocialAccountRepository : GenericRepository<SocialAccount>
    {
        public SocialAccountRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<SocialAccount?> GetByProviderAsync(string provider, string uniqueId)
        {
            return await _context.SocialAccounts
                .Where(s => s.Provider == provider 
                        && s.UniqueId == uniqueId)
                .FirstOrDefaultAsync();
        }
    }
}
