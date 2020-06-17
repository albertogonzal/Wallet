using System;
using System.Threading.Tasks;

namespace Wallet.Core.Interfaces
{
  public interface IBalanceService
  {
    Task Withdraw(Guid userId, string address, decimal amount);
  }
}