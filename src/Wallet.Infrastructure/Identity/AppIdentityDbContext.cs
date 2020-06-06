using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Wallet.Infrastructure.Identity
{
  public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
  {
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
    }
  }
}