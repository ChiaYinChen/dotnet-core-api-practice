using Microsoft.EntityFrameworkCore;
using WebApiApp.Data;
using WebApiApp.Entities;

namespace WebApiApp.Repositories
{
    public class AccountRepository : GenericRepository<Account>
    {
        public AccountRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Account?> GetByEmailAsync(string email)
        {
            return await _context.Accounts.Where(a => a.Email == email).FirstOrDefaultAsync();
        }
    }
}
