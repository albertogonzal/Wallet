using System;
using Wallet.Core.Interfaces;

namespace Wallet.Core.Entities
{
  public class Balance : BaseEntity, IAggregateRoot
  {
    public Balance(Guid userId, decimal amount)
    {
      UserId = userId;
      Amount = amount;
    }

    public Guid UserId { get; }
    public decimal Amount { get; }
  }
}