using System.ComponentModel;
using System;
using Wallet.Core.Entities;

namespace Wallet.Core.Specifications
{
  public class AccountWithAddressesSpecification : BaseSpecification<Account>
  {
    public AccountWithAddressesSpecification() : base(b => true)
    {
      AddInclude(b => b.Addresses);
    }

    public AccountWithAddressesSpecification(Guid accountId) : base(b => b.Id == accountId)
    {
      AddInclude(b => b.Addresses);
    }
  }
}