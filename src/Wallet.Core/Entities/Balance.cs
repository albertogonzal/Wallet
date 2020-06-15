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

    public Guid UserId { get; private set; }
    public decimal Amount { get; private set; }

    public void Deposit(decimal amount)
    {
      Amount += amount;
    }

    public void Withdraw(decimal amount)
    {
      Amount -= amount;
    }
  }
}