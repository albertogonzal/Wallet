using System;
using Wallet.Core.Interfaces;

namespace Wallet.Core.Entities
{
  public class Balance : BaseEntity, IAggregateRoot
  {
    public Balance(Guid userId, string amount)
    {
      UserId = userId;
      Amount = amount;
    }

    public Guid UserId { get; }
    public string Amount { get; }
  }
}