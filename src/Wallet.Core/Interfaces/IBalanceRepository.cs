using System;
using System.Threading.Tasks;
using Wallet.Core.Entities;

namespace Wallet.Core.Interfaces
{
  public interface IBalanceRepository : IAsyncRepository<Balance>
  {
    Task<Balance> GetByUserIdAsync(Guid userId);
  }
}