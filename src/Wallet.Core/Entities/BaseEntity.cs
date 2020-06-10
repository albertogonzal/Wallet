using System;

namespace Wallet.Core.Entities
{
  public abstract class BaseEntity
  {
    public Guid Id { get; protected set; }
  }
}