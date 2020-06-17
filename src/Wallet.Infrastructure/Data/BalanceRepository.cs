using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wallet.Core.Entities;
using Wallet.Core.Interfaces;

namespace Wallet.Infrastructure.Data
{
  public class BalanceRepository : EfRepository<Balance>, IBalanceRepository
  {
    public BalanceRepository(WalletDbContext dbContext) : base(dbContext) { }

    public Task<Balance> GetByUserIdAsync(Guid userId)
    {
      return _dbContext.Set<Balance>().FirstOrDefaultAsync(b => b.UserId == userId);
    }
  }
}