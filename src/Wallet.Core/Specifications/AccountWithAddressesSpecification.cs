using System;
using Wallet.Core.Entities;

namespace Wallet.Core.Specifications
{
  public class AccountWithAddressesSpecification : BaseSpecification<Account>
  {
    public AccountWithAddressesSpecification(Guid accountId) : base(b => b.Id == accountId)
    {
      AddInclude(b => b.Addresses);
    }
  }
}