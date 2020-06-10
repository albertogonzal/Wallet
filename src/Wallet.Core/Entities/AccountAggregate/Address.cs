using System;
namespace Wallet.Core.Entities
{
  public class Address : BaseEntity
  {
    public Address(Guid accountId, Guid assetId, int addressIndex)
    {
      AccountId = accountId;
      AssetId = assetId;
      AddressIndex = addressIndex;
    }

    public Guid AccountId { get; private set; }
    public Guid AssetId { get; private set; }
    public int AddressIndex { get; private set; }
  }
}