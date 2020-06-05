using Microsoft.EntityFrameworkCore;
using Wallet.Core.Entities;

namespace Wallet.Infrastructure.Data
{
  public class WalletDbContext : DbContext
  {
    public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }
  }
}