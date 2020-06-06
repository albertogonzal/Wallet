using System;
namespace Wallet.Core.Entities
{
  public class Address : BaseEntity
  {
    public Address(Guid accountId, Guid assetId, int addressIndex, string balance)
    {
      AccountId = accountId;
      AssetId = assetId;
      AddressIndex = addressIndex;
      Balance = balance;
    }

    public Guid AccountId { get; private set; }
    public Guid AssetId { get; private set; }
    public int AddressIndex { get; private set; }
    public string Balance { get; private set; }

    public bool HasBalance()
    {
      return Convert.ToDecimal(Balance) > 0;
    }
  }
}