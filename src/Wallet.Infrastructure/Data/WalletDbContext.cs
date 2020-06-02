using Microsoft.EntityFrameworkCore;

namespace Wallet.Infrastructure.Data
{
  public class WalletDbContext : DbContext
  {
    public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options) { }

  }
}