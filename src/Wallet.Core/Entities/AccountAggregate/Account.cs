using System.Linq;
using System.Collections.Generic;
using Wallet.Core.Interfaces;

namespace Wallet.Core.Entities
{
  public class Account : BaseEntity, IAggregateRoot
  {
    private readonly Asset _asset;
    private readonly int _walletIndex;
    private readonly List<Address> _addresses = new List<Address>();

    public Account(Asset asset, int walletIndex)
    {
      _asset = asset;
      _walletIndex = walletIndex;
    }

    public Asset Asset => _asset;
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