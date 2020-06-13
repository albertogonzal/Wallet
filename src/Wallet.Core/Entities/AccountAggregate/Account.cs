using System;
using System.Linq;
using System.Collections.Generic;
using Wallet.Core.Interfaces;

namespace Wallet.Core.Entities
{
  public class Account : BaseEntity, IAggregateRoot
  {
    private readonly List<Address> _addresses = new List<Address>();

    public Account(Guid userId, int accountIndex)
    {
      UserId = userId;
      AccountIndex = accountIndex;
    }

    public Guid UserId { get; private set; }
    public int AccountIndex { get; private set; }
    public IReadOnlyList<Address> Addresses => _addresses.AsReadOnly();

    public void AddAddress(Address newAddress)
    {
      _addresses.Add(newAddress);
    }

    public Address LastAddress()
    {
      return _addresses.Last();
    }
  }
}