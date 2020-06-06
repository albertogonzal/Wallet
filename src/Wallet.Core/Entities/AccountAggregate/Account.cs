using System;
using System.Linq;
using System.Collections.Generic;
using Wallet.Core.Interfaces;

namespace Wallet.Core.Entities
{
  public class Account : BaseEntity, IAggregateRoot
  {
    private readonly List<Address> _addresses = new List<Address>();

    public Account(int accountIndex)
    {
      AccountIndex = accountIndex;
    }

    public int AccountIndex { get; private set; }
    public IReadOnlyList<Address> Addresses => _addresses.AsReadOnly();

    public void AddAddress(Address newAddress)
    {
      _addresses.Add(newAddress);
    }

    public Address LastAddress(Guid assetId)
    {
      return _addresses.Where(a => a.AssetId == assetId).Last();
    }
  }
}