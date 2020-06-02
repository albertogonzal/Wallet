using System;
namespace Wallet.Core.Entities
{
  public class Address : BaseEntity
  {
    private readonly Asset _asset;
    private readonly int _addressIndex;
    private readonly string _balance;

    public Address(Asset asset, int addressIndex, string balance)
    {
      _asset = asset;
      _addressIndex = addressIndex;
      _balance = balance;
    }

    public Asset Asset => _asset;
    public int AddressIndex => _addressIndex;
    public string Balance => _balance;

    public bool HasBalance()
    {
      return Convert.ToDecimal(_balance) > 0;
    }
  }
}