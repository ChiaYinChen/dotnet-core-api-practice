using Microsoft.EntityFrameworkCore;
using WebApiApp.Entities;
using WebApiApp.Helpers;

namespace WebApiApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<SocialAccount> SocialAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Email)
                .IsUnique();

            modelBuilder.Entity<SocialAccount>()
                .HasIndex(s => new { s.Provider, s.UniqueId, s.AccountId })
                .IsUnique();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {

            var entries = ChangeTracker.Entries().Where(E => E.State == EntityState.Added || E.State == EntityState.Modified).ToList();

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property("UpdatedAt").CurrentValue = TimeHelper.UtcNow();
                }
                else if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property("CreatedAt").CurrentValue = TimeHelper.UtcNow();
                    entityEntry.Property("UpdatedAt").CurrentValue = TimeHelper.UtcNow();
                }

            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
