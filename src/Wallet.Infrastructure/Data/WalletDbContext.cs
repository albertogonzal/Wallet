using System.Net.Http.Headers;
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
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionType> TransactionTypes { get; set; }
    public DbSet<TransactionStatus> TransactionStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<Balance>().Property(b => b.Amount).HasColumnType("decimal(29, 2)");

      builder.Entity<Transaction>().Ignore(t => t.Type);
      builder.Entity<Transaction>().Property(t => t.Amount).HasColumnType("decimal(29, 2)");
      builder.Entity<Transaction>().Property<int>("_typeValue").UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction).HasColumnName("Type");
      builder.Entity<Transaction>().Property<int>("_statusValue").UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction).HasColumnName("Status");

      builder.Entity<TransactionType>().Property(t => t.Value).HasColumnType("int");

      builder.Entity<TransactionStatus>().Property(t => t.Value).HasColumnType("int");
    }
  }
}