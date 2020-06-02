using System;
using System.Threading.Tasks;
using Wallet.Core.Entities;

namespace Wallet.Core.Interfaces
{
  public interface IAccountService
  {
    Task<Address> NewAddress(Guid userId, Asset asset);
  }
}