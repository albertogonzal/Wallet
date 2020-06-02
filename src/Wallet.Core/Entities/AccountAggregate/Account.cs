using System.Linq;
using System.Collections.Generic;
using Wallet.Core.Interfaces;

namespace Wallet.Core.Entities
{
  public class Account : BaseEntity, IAggregateRoot
  {
    private readonly int _accountIndex;
    private readonly List<Address> _addresses = new List<Address>();

    public Account(int accountIndex)
    {
      _accountIndex = accountIndex;
    }

    public int AccountIndex => _accountIndex;
    public IReadOnlyList<Address> Addresses => _addresses.AsReadOnly();

    public void AddAddress(Address newAddress)
    {
      _addresses.Add(newAddress);
    }

    public Address LastAddress(Asset asset)
    {
      return _addresses.Where(a => a.Asset.Id == asset.Id).Last();
    }
  }
}