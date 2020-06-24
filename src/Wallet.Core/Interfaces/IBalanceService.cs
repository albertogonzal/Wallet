using System;
using System.Threading.Tasks;

namespace Wallet.Core.Interfaces
{
  public interface IBalanceService
  {
    Task Withdraw(Guid userId, Guid assetId, string address, decimal amount);
  }
}