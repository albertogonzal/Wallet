using System;
namespace Wallet.Core.Entities
{
  public class Address : BaseEntity
  {
    public Address(Guid accountId, Guid assetId, int addressIndex, string publicAddress)
    {
      AccountId = accountId;
      AssetId = assetId;
      AddressIndex = addressIndex;
      PublicAddress = publicAddress;
    }

    public Guid AccountId { get; private set; }
    public Guid AssetId { get; private set; }
    public int AddressIndex { get; private set; }
    public string PublicAddress { get; private set; }
  }
}