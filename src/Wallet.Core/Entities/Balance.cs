using System;
using Wallet.Core.Interfaces;

namespace Wallet.Core.Entities
{
  public class Balance : BaseEntity, IAggregateRoot
  {
    public Balance(Guid userId, Guid assetId, string amount)
    {
      UserId = userId;
      AssetId = assetId;
      Amount = amount;
    }

    public Guid UserId { get; }
    public Guid AssetId { get; }
    public string Amount { get; }
  }
}