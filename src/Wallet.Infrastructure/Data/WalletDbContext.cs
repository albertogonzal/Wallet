using Microsoft.EntityFrameworkCore;
using Wallet.Core.Entities;

namespace Wallet.Infrastructure.Data
{
  public class WalletDbContext : DbContext
  {
    public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<Balance> Balances { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<Balance>(eb => eb.Property(b => b.Amount).HasColumnType("decimal(29, 2)"));
      base.OnModelCreating(builder);
    }
  }
}