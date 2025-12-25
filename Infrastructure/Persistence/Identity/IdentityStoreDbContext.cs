using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Address = Domain.Entities.IdentityModule.Address;
namespace Persistence.Identity
{
    public class IdentityStoreDbContext(DbContextOptions<IdentityStoreDbContext> options) : IdentityDbContext(options)
    {

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Address>().ToTable("Addresses");
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

    }
}
