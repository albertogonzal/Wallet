using System;
using System.Threading.Tasks;
using Wallet.Core.Entities;

namespace Wallet.Core.Interfaces
{
  public interface IAccountService
  {
    Task NewAccount(Guid userId);
    Task<Address> NewAddress(Guid accountId, Guid assetId);
  }
}